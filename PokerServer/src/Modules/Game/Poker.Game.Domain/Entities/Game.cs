using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public sealed class Game : Entity
{
	public GameState GameState { get; private set; }

	public readonly Deck Deck;

#pragma warning disable CS8618
	private Game() { }
#pragma warning restore CS8618
	private Game(GameState gameState, Deck deck)
	{
		GameState = gameState;
		Deck = deck;
	}

	public static Result<Game> StartGame(List<Player> players)
	{
		if (players.Count > 6)
			return Result<Game>.Failure(ResponseList.SixPlayersMaximum);

		var stateResult = GameState.Create(
			players: players,
			currentTurnPlayerPosition: 0,
			dealerPosition: 0,
			minimumRaise: 10
		);

		if (stateResult.IsFailure)
			return Result<Game>.Failure(stateResult.Response);

		var gameState = stateResult.Value!;

		var shuffledDeck = Deck.CreateShuffled();
		List<Hand> hands = new();

		foreach (var player in gameState.Players)
		{
			var hand = Hand.Create(new[] { shuffledDeck.Draw(), shuffledDeck.Draw() });
			player.SetHand(hand);
		}

		var gameRoom = new Game(gameState, shuffledDeck);

		return Result<Game>.Success(gameRoom);
	}


	public Result PlaceBet(string playerId, int amount)
	{
		var playerResult = GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		var player = playerResult.Value!;

		int toCall = GameState.CurrentBet - player.Hand!.Bet;
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

		var result = player.Hand.AddToBet(amount);
		if (result.IsFailure)
			return result;

		GameState.AddToPot(amount);
		player.RemoveFromBalance(amount);

		if (amount > toCall)
			GameState.UpdateCurrentBet(player.Hand.Bet);

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerCheck(string playerId)
	{
		var playerResult = GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerFold(string playerId)
	{
		var playerResult = GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		var player = playerResult.Value!;

		var result = player.Hand!.Fold();
		if (result.IsFailure)
			return result;

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerAllIn(string playerId)
	{
		var playerResult = GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		var player = playerResult.Value!;
		var hand = player.Hand;

		if (player.Balance <= 0)
			return Result.Failure(ResponseList.InsufficientFunds);

		var result = hand!.AllIn(player.Balance);
		if (result.IsFailure)
			return result;

		GameState.AddToPot(player.Balance);

		if (hand.Bet > GameState.CurrentBet)
			GameState.UpdateCurrentBet(hand.Bet);

		AdvanceTurn();
		return Result.Success();
	}

	private Result<Player> GetPlayerIfHisTurn(string playerId)
	{
		if (!GameState.PlayerDictionary.TryGetValue(playerId, out var player))
			return Result<Player>.Failure(ResponseList.PlayerNotInGame);
		if (!IsPlayerTurn(playerId))
			return Result<Player>.Failure(ResponseList.NotYourTurn);

		return Result<Player>.Success(player);
	}

	private bool IsPlayerTurn(string playerId)
	{
		if (GameState.Players[GameState.CurrentTurnPlayerPosition].Id != playerId)
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
		var activePlayers = GameState.Players
			.Where(p => !p.Hand!.IsFolded && !p.Hand.IsAllIn)
			.ToList();

		return activePlayers.All(p => p.Hand!.Bet == GameState.CurrentBet);
	}

	private bool OnlyOneActivePlayer()
	{
		return GameState.Players.Count(p => !p.Hand!.IsFolded) == 1;
	}

	private void HandleShowdown()
	{
		// TODO: Evaluate hands, determine winner(s), distribute pot, reset game.

		//var activePlayers = GameState.Players
		//	.Where(p => !p.Hand!.IsFolded)
		//	.ToList();

		//if (activePlayers.Count == 1)
		//{
		//	var winner = activePlayers[0];
		//	winner.AddToBalance(GameState.CurrentPot);
		//	return;
		//}

		//var evaluated = activePlayers
		//	.Select(p => new
		//	{
		//		Player = p,
		//		Score = HandEvaluator.Evaluate(p.Hand!.Cards.Concat(GameState.CommunityCards))
		//	})
		//	.OrderByDescending(x => x.Score)
		//	.ToList();

		//var topScore = evaluated.First().Score;
		//var winners = evaluated.Where(x => x.Score == topScore).ToList();

		//int share = GameState.CurrentPot / winners.Count();

		//foreach (var winner in winners)
		//{
		//	winner.Player.AddToBalance(share);
		//}
	}
}
