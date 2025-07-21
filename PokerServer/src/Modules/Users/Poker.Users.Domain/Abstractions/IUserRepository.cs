using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Users.Domain.Entities;

namespace Poker.Users.Domain.Abstractions;

public interface IUserRepository : IRepository<User>
{
	Task<User?> GetByEmailAsync(string email);
}
