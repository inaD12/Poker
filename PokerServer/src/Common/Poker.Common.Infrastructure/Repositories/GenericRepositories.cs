using Microsoft.EntityFrameworkCore;
using Poker.Common.Domain;
using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Common.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : Entity
{
	private readonly DbContext _context;
	private DbSet<T> Entities => _context.Set<T>();

	public GenericRepository(DbContext context)
	{
		_context = context;
	}

	public virtual async Task AddAsync(T entity)
	{
		await Entities.AddAsync(entity);
	}

	public virtual void Delete(T entity)
	{
		Entities.Remove(entity);
	}

	public virtual async Task<T?> GetByIdAsync(string id)
	{
		var res = await Entities.FindAsync(id);

		return res;
	}

	public virtual void Update(T entity)
	{
		Entities.Update(entity);
	}
}
