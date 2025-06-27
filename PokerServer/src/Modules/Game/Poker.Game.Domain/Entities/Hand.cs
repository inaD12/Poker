using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public sealed class Hand
{
	public IReadOnlyList<Card> Cards { get; private set; }
	public int Bet { get; private set; }
	public bool IsFolded { get; private set; }
	public bool IsAllIn { get; private set; }

#pragma warning disable CS8618
	private Hand() { }
#pragma warning restore CS8618

	private Hand(IReadOnlyList<Card> cards)
	{
		if (cards.Count != 2)
			throw new ArgumentException("Hand must have exactly 2 cards.");

		Cards = cards;
		Bet = 0;
		IsFolded = false;
		IsAllIn = false;
	}

	public static Hand Create(Card[] cards)
		=> new Hand(cards);

	public Result Fold()
	{
		if (IsFolded)
			return Result.Failure(ResponseList.PlayerAlreadyFolded);

		IsFolded = true;
		return Result.Success();
	}

	public Result AllIn(int amount)
	{
		if (IsAllIn)
			return Result.Failure(ResponseList.PlayerAlreadyAllIn);
		if (IsFolded)
			return Result.Failure(ResponseList.PlayerFolded);
		if (amount < 0)
			return Result.Failure(ResponseList.AmountCantBeNegative);

		IsAllIn = true;
		Bet += amount;

		return Result.Success();
	}

	public Result AddToBet(int amount)
	{
		if (IsAllIn)
			return Result.Failure(ResponseList.PlayerAllIn);
		if (IsFolded)
			return Result.Failure(ResponseList.PlayerFolded);
		if (amount < 0)
			return Result.Failure(ResponseList.AmountCantBeNegative);

		Bet += amount;
		return Result.Success();
	}

	public Result ResetBet()
	{
		if (IsAllIn)
			return Result.Failure(ResponseList.PlayerAllIn);

		Bet = 0;
		return Result.Success();
	}
}
