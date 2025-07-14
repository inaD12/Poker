using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.DTOs;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Responses;
using Poker.Game.Domain.Services;

namespace Poker.Game.Domain.Entities;

public sealed class Game : Entity
{
	public List<Card> CommunityCards { get; private set; }
	public int CurrentPot { get; private set; }
	public int DealerPosition { get; private set; }
	public int CurrentBet { get; private set; }
	public int MinimumRaise { get; private set; }

	public readonly Deck Deck;
	private readonly PlayerManager _playerManager;
	private readonly PhaseManager _phaseManager;

#pragma warning disable CS8618
	private Game() { }
#pragma warning restore CS8618
	private Game(
		List<Card> communityCards,
		int currentPot,
		List<Player> players,
		int currentTurnPlayerPosition,
		int dealerPosition,
		GamePhase phase,
		int currentBet,
		int minimumRaise,
		Deck deck)
	{
		CommunityCards = communityCards;
		CurrentPot = currentPot;
		DealerPosition = dealerPosition;
		CurrentBet = currentBet;
		MinimumRaise = minimumRaise;
		Deck = deck;
		_playerManager = new PlayerManager(players, currentTurnPlayerPosition);
		_phaseManager = new PhaseManager(phase);
	}

	public static Result<Game> StartGame(List<Player> players)
	{
		switch (players.Count)
		{
			case > 6:
				return Result<Game>.Failure(ResponseList.SixPlayersMaximum);
			case < 2:
				return Result<Game>.Failure(ResponseList.TwoPlayersRequired);
		}

		var shuffledDeck = Deck.CreateShuffled();

		foreach (var player in players)
		{
			var hand = Hand.Create([shuffledDeck.Draw(), shuffledDeck.Draw()]);
			player.SetHand(hand);
		}

		var gameRoom = new Game(
			communityCards: new List<Card>(),
			currentPot: 0,
			players: players,
			currentTurnPlayerPosition: 0,
			dealerPosition: 0,
			phase: GamePhase.PreFlop,
			currentBet: 0,
			minimumRaise: 10,
			shuffledDeck);

		return Result<Game>.Success(gameRoom);
	}

	public Result PlayerPlaceBet(string playerId, int amount)
	{
		var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		if (amount < 0)
			return Result.Failure(ResponseList.AmountCantBeNegative);

		var player = playerResult.Value!;

		int toCall = CurrentBet - player.Hand!.Bet;
		if (amount < toCall)
			return Result.Failure(ResponseList.BetTooSmall);

		if (amount > toCall)
		{
			int raiseAmount = amount - toCall;
			if (raiseAmount < MinimumRaise)
				return Result.Failure(ResponseList.MinimumRaiseNotMet);
		}

		if (player.Balance < amount)
			return Result.Failure(ResponseList.InsufficientFunds);

		var result = player.Hand.AddToBet(amount);
		if (result.IsFailure)
			return result;

		CurrentPot += amount;
		player.RemoveFromBalance(amount);
		CurrentBet = player.Hand.Bet;

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerCheck(string playerId)
	{
		var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		AdvanceTurn();
		return Result.Success();
	}

	public Result PlayerFold(string playerId)
	{
		var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
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
		var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);

		var player = playerResult.Value!;
		var hand = player.Hand;

		if (player.Balance <= 0)
			return Result.Failure(ResponseList.InsufficientFunds);

		var result = hand!.AllIn(player.Balance);
		if (result.IsFailure)
			return result;

		CurrentPot += player.Balance;
		player.RemoveFromBalance(player.Balance);

		if (hand.Bet > CurrentBet)
			CurrentBet = hand.Bet;

		AdvanceTurn();
		return Result.Success();
	}

	public GameStateDto GetGameState(string requestingPlayerId)
	{
		var players = _playerManager.GetPlayers()
			.Select(p => new PlayerStateDto(
				Id: p.Id,
				Balance: p.Balance,
				IsFolded: p.Hand?.IsFolded ?? false,
				IsAllIn: p.Hand?.IsAllIn ?? false,
				CurrentBet: p.Hand?.Bet ?? 0,
				IsCurrentTurn: _playerManager.IsPlayerTurn(p.Id),
				Cards: p.Id == requestingPlayerId ? p.Hand?.Cards.ToList() : null
			))
			.ToList();

		return new GameStateDto(
			Phase: _phaseManager.CurrentPhase,
			CommunityCards: CommunityCards.AsReadOnly(),
			CurrentPot: CurrentPot,
			CurrentBet: CurrentBet,
			MinimumRaise: MinimumRaise,
			CurrentTurnPlayerId: _playerManager.GetCurrentTurnPlayer()?.Id,
			Players: players
		);
	}

	private void AdvanceTurn()
	{
		if (_playerManager.OnlyOneActivePlayer())
		{
			HandleShowdown();
		}
		else if (_playerManager.IsBettingRoundComplete(CurrentBet))
		{
			_phaseManager.AdvancePhase(CommunityCards, Deck, HandleShowdown);
			_playerManager.ResetHandsForNextRound();
			CurrentBet = 0;
			_playerManager.SetFirstActivePlayer();
		}
		else
		{
			_playerManager.GetNextActivePosition();
		}
	}

	private void HandleShowdown()
	{
		// TODO: Reset game.
		var players = _playerManager.GetPlayers();

		var activePlayers = players
			.Where(p => !p.Hand!.IsFolded)
			.ToList();

		if (activePlayers.Count == 1)
		{
			var winner = activePlayers[0];
			winner.AddToBalance(CurrentPot);
			return;
		}

		var evaluated = activePlayers
			.Select(p => new
			{
				Player = p,
				Score = HandEvaluator.EvaluateHand(p.Hand!.Cards.Concat(CommunityCards).ToList())
			})
			.OrderByDescending(x => x.Score)
			.ToList();

		var topScore = evaluated.First().Score;
		var winners = evaluated.Where(x => x.Score == topScore).ToList();

		int share = CurrentPot / winners.Count();

		foreach (var winner in winners)
		{
			winner.Player.AddToBalance(share);
		}
	}
}
