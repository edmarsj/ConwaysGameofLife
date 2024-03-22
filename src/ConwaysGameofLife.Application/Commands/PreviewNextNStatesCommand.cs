using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Models;
using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record PreviewNextNStatesCommand(Guid BoardId, int NumStates) : IRequest<BoardQueryResult>
    {
    }
}
