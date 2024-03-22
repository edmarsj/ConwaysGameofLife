using ConwaysGameofLife.Domain.Models;
using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record PreviewBoardGenerationCommand(Guid BoardId, int Generation) : IRequest<BoardQueryResult>;

}
