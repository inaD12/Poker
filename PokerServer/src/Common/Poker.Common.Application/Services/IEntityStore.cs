namespace Poker.Common.Application.Services;

public interface IEntityStore<T>
{
    Task<T?> GetAsync(string id, CancellationToken cancellationToken);
    Task SaveNewAsync(T entity, CancellationToken cancellationToken);
    Task SaveAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}