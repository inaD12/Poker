using Poker.Common.Domain.Abstractions;
using Poker.Users.Domain.Entities;

namespace Poker.Users.Domain.Abstractions;

public interface IUserRepository : IGenericRepository<User>
{
	Task<User?> GetByEmailAsync(string email);
}
