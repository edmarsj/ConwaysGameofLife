using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Entities;
using ConwaysGameofLife.Domain.Models;
using ConwaysGameofLife.Domain.Repositories;
using MediatR;

namespace ConwaysGameofLife.Application.Commands.Handlers
{
    internal class ListAllBoardsCommandHandler : IRequestHandler<ListAllBoardsCommand, IEnumerable<BoardListEntry>>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;

        public ListAllBoardsCommandHandler(IRepository<BoardStateSnapshot> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BoardListEntry>> Handle(ListAllBoardsCommand request, CancellationToken cancellationToken)
        {
            var result = new List<BoardListEntry>();

            var queryResults = await _repository.GetAllAync(cancellationToken);

            result.AddRange(queryResults.Select(m => new BoardListEntry(
                BoardId: m.Id,
                BoardName: m.Name,
                AsciiRepresentation: BoardState.FromSnapshotString(m.Snapshot).ToAscii()
                )));

            return result;
        }
    }
}
