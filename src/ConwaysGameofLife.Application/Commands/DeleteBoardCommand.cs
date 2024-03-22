using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record DeleteBoardCommand(Guid BoardId) : IRequest;
}
