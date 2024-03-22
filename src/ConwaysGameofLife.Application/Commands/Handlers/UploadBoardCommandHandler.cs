using ConwaysGameofLife.Domain.DTOS;
using ConwaysGameofLife.Domain.Entities;
using ConwaysGameofLife.Domain.Exceptions;
using ConwaysGameofLife.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwaysGameofLife.Application.Commands.Handlers
{
    internal class UploadBoardCommandHandler : IRequestHandler<UploadBoardCommand, Guid>
    {
        private readonly IRepository<BoardStateSnapshot> _repository;

        public UploadBoardCommandHandler(IRepository<BoardStateSnapshot> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(UploadBoardCommand request, CancellationToken cancellationToken)
        {
            var state = BoardState.FromFileContent(request.FileContent);

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
