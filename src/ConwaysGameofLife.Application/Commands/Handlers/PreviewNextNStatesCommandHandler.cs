using ConwaysGameofLife.Application.Services;
using ConwaysGameofLife.Core.Exceptions;
using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Entities;
using ConwaysGameofLife.Domain.Exceptions;
using ConwaysGameofLife.Domain.Models;
using ConwaysGameofLife.Domain.Repositories;
using MediatR;
using System.Diagnostics;

namespace ConwaysGameofLife.Application.Commands.Handlers
{
    internal class PreviewNextNStatesCommandHandler : IRequestHandler<PreviewNextNStatesCommand, BoardQueryResult>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;
        private readonly IGameOfLifeService _gameOfLifeService;

        public PreviewNextNStatesCommandHandler(IRepository<BoardStateSnapshot> repository,
                                                IGameOfLifeService gameOfLifeService)
        {
            _repository = repository;
            _gameOfLifeService = gameOfLifeService;
        }

        public async Task<BoardQueryResult> Handle(PreviewNextNStatesCommand request, CancellationToken cancellationToken)
        {
            if (request.NumStates <= 0)
            {
                throw new InvalidNumStatesException();                
            }

            // Get entity
            var entity = await _repository.GetAsync(request.BoardId.ToString(), cancellationToken)
                ?? throw new BoardNotFoundException();

            // Load state
            var timer = Stopwatch.StartNew();
            var state = BoardState.FromSnapshotString(entity.Snapshot);
            var result = _gameOfLifeService.GetBoardGenerations(state, request.NumStates);
            timer.Stop();

            return new BoardQueryResult(entity.Name,
                                        result,
                                        timer.ElapsedMilliseconds);
        }
    }
}
