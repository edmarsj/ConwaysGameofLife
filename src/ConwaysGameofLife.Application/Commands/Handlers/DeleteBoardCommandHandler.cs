using ConwaysGameofLife.Domain.Entities;
using ConwaysGameofLife.Domain.Exceptions;
using ConwaysGameofLife.Domain.Repositories;
using MediatR;

namespace ConwaysGameofLife.Application.Commands.Handlers
{
    internal class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;

        public DeleteBoardCommandHandler(IRepository<BoardStateSnapshot> repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.GetAsync(request.BoardId.ToString(), cancellationToken) == null)
            {
                throw new BoardNotFoundException();
            }

            await _repository.DeleteAsync(request.BoardId.ToString(), cancellationToken);
        }
    }
}
