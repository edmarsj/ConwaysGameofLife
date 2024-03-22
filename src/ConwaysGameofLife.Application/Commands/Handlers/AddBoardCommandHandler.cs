using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Entities;
using ConwaysGameofLife.Domain.Repositories;
using MediatR;

namespace ConwaysGameofLife.Application.Commands.Handlers
{
    internal class AddBoardCommandHandler : IRequestHandler<AddBoardCommand, Guid>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;

        public AddBoardCommandHandler(IRepository<BoardStateSnapshot> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(AddBoardCommand request, CancellationToken cancellationToken)
        {
            var state = BoardState.FromMatrix(request.BoardData);

            var newId = Guid.NewGuid();
            var entity = new BoardStateSnapshot
            {
                Id = newId.ToString(),
                Name = request.Name,
                Snapshot = state.ToSnapshotString()
            };

            await _repository.SaveAsync(entity, cancellationToken);

            return newId;
        }
    }
}
