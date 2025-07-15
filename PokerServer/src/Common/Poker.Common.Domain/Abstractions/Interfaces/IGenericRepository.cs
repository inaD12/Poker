namespace Poker.Common.Domain.Abstractions.Interfaces;

public interface IGenericRepository<T> where T : class
{
	Task AddAsync(T entity);
	void Delete(T entity);
	Task<T?> GetByIdAsync(string id);
	void Update(T entity);
}