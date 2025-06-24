using Poker.Users.Domain.Abstractions.Auth.Models;

namespace Poker.Users.Domain.Abstractions.Auth;

public interface IPasswordManager
{
	PasswordHashResult HashPassword(string password);
	bool VerifyPassword(string password, string hash, string salt);
}