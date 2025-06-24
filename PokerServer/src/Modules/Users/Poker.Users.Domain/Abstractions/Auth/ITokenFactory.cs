using Poker.Users.Domain.Abstractions.Auth.Models;

namespace Poker.Users.Domain.Abstractions.Auth;

public interface ITokenFactory
{
	TokenResult CreateToken(string id);
}