using System.Security.Cryptography;
using Poker.Users.Domain.Abstractions.Auth;
using Poker.Users.Domain.Abstractions.Auth.Models;

namespace Poker.Users.Infrastructure.Features.Auth;

public class PasswordManager : IPasswordManager
{
    private const int KeySize = 64;
    private const int Iterations = 1000;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public PasswordHashResult HashPassword(string password)
    {
        var saltByteArray = RandomNumberGenerator.GetBytes(KeySize);
        var salt = Convert.ToHexString(saltByteArray);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltByteArray,
            Iterations,
            _hashAlgorithm,
            KeySize);

        var stringHash = Convert.ToHexString(hash);

        return new PasswordHashResult(stringHash, salt);
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        var hashFromPass = Rfc2898DeriveBytes.Pbkdf2(
            password,
            Convert.FromHexString(salt),
            Iterations,
            _hashAlgorithm,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(hashFromPass, Convert.FromHexString(hash));
    }
}