using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Domain.Entities;

public sealed class User : Entity
{
#pragma warning disable CS8618
    private User()
#pragma warning restore CS8618
    {
    }

    private User(
        string email,
        string passwordHash,
        string salt,
        string username,
        int gamesPlayed,
        int gamesWon,
        decimal totalEarnings)
    {
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
        Username = username;
        GamesPlayed = gamesPlayed;
        GamesWon = gamesWon;
        TotalEarnings = totalEarnings;
    }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Salt { get; private set; }
    public string Username { get; private set; }
    public int GamesPlayed { get; private set; }
    public int GamesWon { get; private set; }
    public decimal TotalEarnings { get; private set; }

    public static User Create(
        string email,
        string passwordHash,
        string salt,
        string username
    )
    {
        var user = new User(
            email,
            passwordHash,
            salt,
            username,
            0,
            0,
            0);


        return user;
    }

    public Result UpdateUsername(string newUsername)
    {
        if (Username == newUsername)
            return Result.Failure(ResponseList.SameUsername);

        Username = newUsername;

        return Result.Success();
    }

    public void PlayedGame(bool won = false, decimal? earnings = null)
    {
        GamesPlayed++;

        if (won)
        {
            GamesWon++;

            if (earnings is > 0)
                TotalEarnings += earnings.Value;
        }
    }
}