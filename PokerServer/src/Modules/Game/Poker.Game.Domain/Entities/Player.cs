using Poker.Common.Domain;

namespace Poker.Game.Domain.Entities;

public sealed class Player: Entity
{
	public string Username { get; private set; }
	public decimal Balance { get; private set; }

	private Player() { }

	private Player(string username, decimal balance)
	{
		Username = username;
		Balance = balance;
	}

	public static Player Create(string username, decimal balance)
	{
		if (string.IsNullOrWhiteSpace(username))
			throw new ArgumentException("Username cannot be empty.", nameof(username));
		if (balance < 0)
			throw new ArgumentException("Balance cannot be negative.", nameof(balance));

		return new Player(username, balance);
	}
}
