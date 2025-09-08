namespace Poker.Common.Domain.Abstractions.Interfaces;

public interface IRepository<T> where T : Entity
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task DeleteByIdAsync(string id, CancellationToken cancellationToken);
    void Delete(T entity);
}