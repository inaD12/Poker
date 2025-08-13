using Newtonsoft.Json;
using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Player : Entity
{
    private Player()
    {
    }

    [JsonConstructor]
    private Player(string username, int balance, Hand? hand, int gamesPlayed, int gamesWon, decimal totalEarnings, bool isDisconnected)
    {
        Username = username;
        Balance = balance;
        Hand = hand;
        GamesPlayed = gamesPlayed;
        GamesWon = gamesWon;
        TotalEarnings = totalEarnings;
        IsDisconnected = isDisconnected;
    }

    public string Username { get; private set; }
    public int Balance { get; private set; }
    public Hand? Hand { get; private set; }
    
    public int GamesPlayed { get; private set; }
    public int GamesWon { get; private set; }
    public decimal TotalEarnings { get; private set; }
    public bool IsDisconnected { get; private set; }

    public void Disconnect()
    {
        IsDisconnected = true;
    }

    public void Reconnect()
    {
        IsDisconnected = false;
    }

    public static Result<Player> Create(string username, int balance)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result<Player>.Failure(ResponseList.UsernameEmpty);
        if (balance < 0)
            return Result<Player>.Failure(ResponseList.BalanceNegative);

        var player = new Player(username, balance, null, 0, 0, 0, false);
        return Result<Player>.Success(player);
    }

    internal void SetHand(Hand hand)
    {
        Hand = hand;
    }

    internal void AddToBalance(int amount)
    {
        Balance += amount;
    }

    internal Result RemoveFromBalance(int amount)
    {
        if (amount > Balance)
            return Result.Failure(ResponseList.InsufficientFunds);

        Balance -= amount;
        return Result.Success();
    }
}