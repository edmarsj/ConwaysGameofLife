using ConwaysGameofLife.Domain.Entities;

namespace ConwaysGameofLife.Domain.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAync(CancellationToken cancellationToken = default);
        Task<T> GetAsync(object hashKey, CancellationToken cancellationToken = default);        
        Task SaveAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(object hashKey, CancellationToken cancellationToken = default);
    }
}