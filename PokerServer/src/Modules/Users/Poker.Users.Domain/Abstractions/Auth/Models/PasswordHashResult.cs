namespace Poker.Users.Domain.Abstractions.Auth.Models;

public record PasswordHashResult(string PasswordHash, string Salt);