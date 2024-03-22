using ConwaysGameofLife.Application.Services;
using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Entities;
using ConwaysGameofLife.Domain.Exceptions;
using ConwaysGameofLife.Domain.Models;
using ConwaysGameofLife.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace ConwaysGameofLife.Application.Commands.Handlers
{
    internal class PreviewBoardGenerationCommandHandler : IRequestHandler<PreviewBoardGenerationCommand, BoardQueryResult>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;
        private readonly IGameOfLifeService _gameOfLifeService;
        private readonly IMemoryCache _memoryCache;
        public PreviewBoardGenerationCommandHandler(IRepository<BoardStateSnapshot> repository,
                                                    IGameOfLifeService gameOfLifeService,
                                                    IMemoryCache memoryCache)
        {
            _repository = repository;
            _gameOfLifeService = gameOfLifeService;
            _memoryCache = memoryCache;
        }

        public async Task<BoardQueryResult> Handle(PreviewBoardGenerationCommand request, CancellationToken cancellationToken)
        {
            // Get entity
            // It is valuable to cache this request as command can be called a lot of times - like in showGenerations.ps1
            if (!_memoryCache.TryGetValue(request.BoardId, out BoardStateSnapshot entity))
            {
                entity = await _repository.GetAsync(request.BoardId.ToString(), cancellationToken);
                _memoryCache.Set(request.BoardId, entity,TimeSpan.FromMinutes(5));
            }

            if (entity == null)
            {
                throw new BoardNotFoundException();
            }

            // Load state
            var timer = Stopwatch.StartNew();
            var state = BoardState.FromSnapshotString(entity.Snapshot);
            var result = _gameOfLifeService.GetBoardGeneration(state, request.Generation);
            timer.Stop();

            return new BoardQueryResult(entity.Name,
                                        new[] { result },
                                        timer.ElapsedMilliseconds);
        }
    }
}
