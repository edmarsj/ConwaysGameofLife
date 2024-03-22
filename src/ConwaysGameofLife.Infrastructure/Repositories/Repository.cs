using Amazon.DynamoDBv2.DataModel;
using ConwaysGameofLife.Domain.Entities;

namespace ConwaysGameofLife.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly IDynamoDBContext _context;

        public Repository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(object hashKey, CancellationToken cancellationToken = default)
        {
            await _context.DeleteAsync<T>(hashKey, cancellationToken);            
        }

        public async Task<IEnumerable<T>> GetAllAync(CancellationToken cancellationToken = default)
        {
            return await _context.ScanAsync<T>(default)
                                 .GetRemainingAsync(cancellationToken);
        }

        public async Task<T> GetAsync(object hashKey, CancellationToken cancellationToken = default)
        {
            return await _context.LoadAsync<T>(hashKey, cancellationToken);
        }
        public async Task SaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.SaveAsync(entity, cancellationToken);
        }
    }
}
