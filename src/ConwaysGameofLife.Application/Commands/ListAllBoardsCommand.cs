using ConwaysGameofLife.Domain.Models;
using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record ListAllBoardsCommand : IRequest<IEnumerable<BoardListEntry>>;
    
}
