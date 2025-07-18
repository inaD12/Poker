using Microsoft.EntityFrameworkCore;
using Poker.Common.Infrastructure.Repositories;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Entities;
using Poker.Users.Infrastructure.Features.DBContexts;

namespace Poker.Users.Infrastructure.Features.Repositories;

internal class UserRepository : GenericRepository<User>, IUserRepository
{
	private readonly UsersDBContext _context;

	public UserRepository(UsersDBContext context) : base(context)
	{
		_context = context;
	}

	// public async Task<PagedList<User>?> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken)
	// {
	// 	var entitiesQuery = _context.Users
	// 		.Where(u =>
	// 			(string.IsNullOrEmpty(query.FirstName) || u.FirstName.StartsWith(query.FirstName)) &&
	// 			(string.IsNullOrEmpty(query.LastName) || u.LastName.StartsWith(query.LastName)) &&
	// 			(string.IsNullOrEmpty(query.Email) || u.Email.StartsWith(query.Email)) &&
	// 			(!query.EmailVerified.HasValue || u.EmailVerified == query.EmailVerified!.Value)
	// 		).ApplySorting(query.SortPropertyName, query.SortOrder);
	//
	// 	if (entitiesQuery.IsNullOrEmpty())
	// 		return null;
	//
	// 	var users = await PagedList<User>.CreateAsync(entitiesQuery, query.Page, query.PageSize, cancellationToken);
	// 	return users;
	// }

	public async Task<User?> GetByEmailAsync(string email)
	{
		var user = await _context.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.Email == email);

		return user;
	}
}