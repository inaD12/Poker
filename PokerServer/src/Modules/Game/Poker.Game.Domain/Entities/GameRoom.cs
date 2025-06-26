using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public sealed class GameRoom : Entity
{
	public Dictionary<string, Player> Players { get; private set; }
	public GameState GameState { get; private set; }

	private Deck Deck;

#pragma warning disable CS8618
	private GameRoom() { }
#pragma warning restore CS8618
	private GameRoom(List<Player> players, GameState gameState, Deck deck)
	{
		Players = players.ToDictionary(p => p.Id);
		GameState = gameState;
		Deck = deck;
	}

	public static Result<GameRoom> StartGame(List<Player> players)
	{
		if (players.Count > 6)
			return Result<GameRoom>.Failure(ResponseList.SixPlayersMaximum);

		var stateResult = GameState.Create(
			playerOrder: players.Select(p => p.Id).ToList(),
			currentTurnPlayerPosition: 0,
			dealerPosition: 0,
			minimumRaise: 10
		);

		if (stateResult.IsFailure)
			return Result<GameRoom>.Failure(stateResult.Response);

		var gameState = stateResult.Value!;

		var shuffledDeck = Deck.CreateShuffled();
		var hands = players.Select(p =>
			Hand.Create(p.Id, new[] { shuffledDeck.Draw(), shuffledDeck.Draw() })).ToList();

		gameState.DealHands(hands);

		var gameRoom = new GameRoom(players, gameState, shuffledDeck);

		return Result<GameRoom>.Success(gameRoom);
	}


	public Result PlaceBet(string playerId, int amount)
	{
		if (!GameState.Hands.TryGetValue(playerId, out var hand) || !Players.TryGetValue(playerId, out var player))
			return Result.Failure(ResponseList.PlayerNotInGame);
		if(!IsPlayerTurn(playerId))
			return Result.Failure(ResponseList.NotYourTurn);

		int toCall = GameState.CurrentBet - hand!.Bet;
		if (amount < toCall)
			return Result.Failure(ResponseList.BetTooSmall);

		if (amount > toCall)
		{
			int raiseAmount = amount - toCall;
			if (raiseAmount < GameState.MinimumRaise)
				return Result.Failure(ResponseList.MinimumRaiseNotMet);
		}

		if (player.Balance < amount)
			return Result.Failure(ResponseList.InsufficientFunds);

		var result = hand.AddToBet(amount);
		if (result.IsFailure)
			return result;

		GameState.AddToPot(amount);
		player.RemoveFromBalance(amount);

		if (amount > toCall)
			GameState.UpdateCurrentBet(hand.Bet);

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerCheck(string playerId)
	{
		if (!GameState.Hands.ContainsKey(playerId))
			return Result.Failure(ResponseList.PlayerNotInGame);
		if (!IsPlayerTurn(playerId))
			return Result.Failure(ResponseList.NotYourTurn);

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerFold(string playerId)
	{
		if (!GameState.Hands.TryGetValue(playerId, out Hand hand))
			return Result.Failure(ResponseList.PlayerNotInGame);
		if (!IsPlayerTurn(playerId))
			return Result.Failure(ResponseList.NotYourTurn);

		var result = hand.Fold();
		if (result.IsFailure)
			return result;

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerAllIn(string playerId)
	{
		if (!GameState.Hands.TryGetValue(playerId, out var hand) || !Players.TryGetValue(playerId, out var player))
			return Result.Failure(ResponseList.PlayerNotInGame);
		if (!IsPlayerTurn(playerId))
			return Result.Failure(ResponseList.NotYourTurn);

		if (player.Balance <= 0)
			return Result.Failure(ResponseList.InsufficientFunds);

		var result = hand.AllIn(player.Balance);
		if (result.IsFailure)
			return result;

		GameState.AddToPot(player.Balance);

		if (hand.Bet > GameState.CurrentBet)
			GameState.UpdateCurrentBet(hand.Bet);

		AdvanceTurn();
		return Result.Success();
	}

	private bool IsPlayerTurn(string playerId)
	{
		if (GameState.PlayerOrder[GameState.CurrentTurnPlayerPosition] != playerId)
			return false;

		return true;
	}

	private void AdvanceGamePhase()
	{
		switch (GameState.Phase)
		{
			case GamePhase.PreFlop:
				GameState.Flop(new List<Card> { Deck.Draw(), Deck.Draw(), Deck.Draw() });
				break;
			case GamePhase.Flop:
				GameState.Turn(Deck.Draw());
				break;
			case GamePhase.Turn:
				GameState.River(Deck.Draw());
				break;
			case GamePhase.River:
				HandleShowdown();
				break;
		}
	}

	private void AdvanceTurn()
	{
		if (OnlyOneActivePlayer())
		{
			HandleShowdown();
		}
		else if (IsBettingRoundComplete())
		{
			AdvanceGamePhase();
			GameState.ResetBetsForNextRound();
			GameState.SetFirstActivePlayer();
		}
		else
		{
		GameState.NextPlayer();
		}
	}

	private bool IsBettingRoundComplete()
	{
		var activePlayers = GameState.Hands.Values
			.Where(h => !h.IsFolded && !h.IsAllIn)
			.ToList();

		return activePlayers.All(h => h.Bet == GameState.CurrentBet);
	}

	private bool OnlyOneActivePlayer()
	{
		return GameState.Hands.Values.Count(h => !h.IsFolded) == 1;
	}

	private void HandleShowdown()
	{
		// TODO: Evaluate hands, determine winner(s), distribute pot, reset game.
	}
}
