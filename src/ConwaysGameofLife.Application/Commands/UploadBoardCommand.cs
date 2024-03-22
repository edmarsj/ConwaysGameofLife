using MediatR;

namespace ConwaysGameofLife.Application.Commands
{
    public record UploadBoardCommand(string Name, string FileContent) : IRequest<Guid>;
}
