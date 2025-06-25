using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public sealed class GameRoom: Entity
{
	public List <Player> Players { get; private set; } = new List<Player>(6);

	public GameState GameState { get; private set; }

	public Result PlaceBet(string playerId, int amount)
	{
		if (!GameState.Hands.TryGetValue(playerId, out Hand hand))
			return Result.Failure(ResponseList.PlayerNotInGame);

		int toCall = GameState.CurrentBet - hand.Bet;
		if (amount < toCall)
			return Result.Failure(ResponseList.BetTooSmall);

		if (amount > toCall)
		{
			int raiseAmount = amount - toCall;
			if (raiseAmount < GameState.MinimumRaise)
				return Result.Failure(ResponseList.MinimumRaiseNotMet);
		}

		var result = hand.AddToBet(amount);
		if (result.IsFailure)
			return result;

		GameState.AddToPot(amount);

		if (amount > toCall)
			GameState.UpdateCurrentBet(hand.Bet);

		return Result.Success();
	}
}
