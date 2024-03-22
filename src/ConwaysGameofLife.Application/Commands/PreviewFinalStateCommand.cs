using ConwaysGameofLife.Domain.Models;
using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record PreviewFinalStateCommand(Guid BoardId, int MaxSteps) : IRequest<FinalBoardStateResult>
    {
    }
}
