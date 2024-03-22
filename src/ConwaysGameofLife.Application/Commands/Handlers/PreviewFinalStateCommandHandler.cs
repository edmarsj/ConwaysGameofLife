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
    internal class PreviewFinalStateCommandHandler : IRequestHandler<PreviewFinalStateCommand, FinalBoardStateResult>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;
        private readonly IGameOfLifeService _gameOfLifeService;

        public PreviewFinalStateCommandHandler(IRepository<BoardStateSnapshot> repository,
                                               IGameOfLifeService gameOfLifeService)
        {
            _repository = repository;
            _gameOfLifeService = gameOfLifeService;
        }

        public async Task<FinalBoardStateResult> Handle(PreviewFinalStateCommand request, CancellationToken cancellationToken)
        {
            if (request.MaxSteps <= 0)
            {
                throw new InvalidMaxStepsException();
            }

            // Get entity
            var entity = await _repository.GetAsync(request.BoardId.ToString(), cancellationToken)
                ?? throw new BoardNotFoundException();

            var timer = Stopwatch.StartNew();
            var state = BoardState.FromSnapshotString(entity.Snapshot);
            var result = _gameOfLifeService.GetFinalState(state, request.MaxSteps);
            timer.Stop();

            return new FinalBoardStateResult(entity.Name, result.numGenerations, result.finalState, timer.ElapsedMilliseconds);
        }
    }
}
