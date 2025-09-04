using Microsoft.EntityFrameworkCore;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Entities;
using Poker.Users.Infrastructure.Features.DBContexts;

namespace Poker.Users.Infrastructure.Features.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
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

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync([id], cancellationToken);
        if (user != null)
            _context.Users.Remove(user);
    }

    public void Delete(User user)
    {
        _context.Remove(user);
    }
}