using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public sealed class Player: Entity
{
	public string Username { get; private set; } = null!;
	public int Balance { get; private set; }
	public Hand? Hand { get; private set; }

	private Player() { }

	private Player(string username, int balance, Hand? hand)
	{
		Username = username;
		Balance = balance;
		Hand = hand;
	}

	public static Player Create(string username, int balance)
	{
		if (string.IsNullOrWhiteSpace(username))
			throw new ArgumentException("Username cannot be empty.", nameof(username));
		if (balance < 0)
			throw new ArgumentException("Balance cannot be negative.", nameof(balance));

		return new Player(username, balance, null);
	}

	public void SetHand(Hand hand)
	{
		Hand = hand;
	}

	public void AddToBalance(int amount)
	{
		Balance += amount;
	}

	public Result RemoveFromBalance(int amount) {
		if (amount > Balance)
			return Result.Failure(ResponseList.InsufficientFunds);

		Balance -= amount;
		return Result.Success();
	}
}
