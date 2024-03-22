using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record AddBoardCommand(string Name, int[,] BoardData) : IRequest<Guid>;
}
