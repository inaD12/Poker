using Poker.Common.Domain;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Events;
using Poker.Game.Domain.Responses;
using Poker.Game.Domain.Services;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Table : Entity
{
	public List<Card> CommunityCards { get; private set; }
	public int CurrentPot { get; private set; }
	public int DealerPosition { get; private set; }
	public int CurrentBet { get; private set; }
	public int MinimumRaise { get; private set; }
	public GamePhase CurrentPhase { get; private set; }

	private readonly Deck _deck;
	private readonly PlayerManager _playerManager;

#pragma warning disable CS8618
	private Table() { }
#pragma warning restore CS8618
	private Table(
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
		CurrentPhase = phase;
		CurrentBet = currentBet;
		MinimumRaise = minimumRaise;
		_deck = deck;
		_playerManager = new PlayerManager(players, currentTurnPlayerPosition);
	}

	public static Result<Table> StartGame(List<Player> players)
	{
		switch (players.Count)
		{
			case > 6:
				return Result<Table>.Failure(ResponseList.SixPlayersMaximum);
			case < 2:
				return Result<Table>.Failure(ResponseList.TwoPlayersRequired);
		}

		var shuffledDeck = Deck.CreateShuffled();

		foreach (var player in players)
		{
			var hand = Hand.Create([shuffledDeck.Draw(), shuffledDeck.Draw()]);
			player.SetHand(hand);
		}

		var gameRoom = new Table(
			communityCards: new List<Card>(),
			currentPot: 0,
			players: players,
			currentTurnPlayerPosition: 0,
			dealerPosition: 0,
			phase: GamePhase.PreFlop,
			currentBet: 0,
			minimumRaise: 10,
			shuffledDeck);

		return Result<Table>.Success(gameRoom);
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
		
		RaiseDomainEvent(new PlayerTookActionDomainEvent(
			Id,
			playerId,
			PlayerActionType.PlaceBet,
			_playerManager.GetCurrentTurnPlayer().Id,
			amount ));
		
		return Result.Success();
	}

	public Result PlayerCheck(string playerId)
	{
		var playerResult = _playerManager.GetPlayerIfHisTurn(playerId);
		if (playerResult.IsFailure)
			return Result.Failure(playerResult.Response);
		
		if(playerResult.Value!.Hand!.Bet != CurrentBet)
			return Result.Failure(ResponseList.MustMatchBet);
		
		AdvanceTurn();
		
		RaiseDomainEvent(new PlayerTookActionDomainEvent(
			Id,
			playerId,
			PlayerActionType.Check,
			_playerManager.GetCurrentTurnPlayer().Id));
		
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
		
		RaiseDomainEvent(new PlayerTookActionDomainEvent(
			Id,
			playerId,
			PlayerActionType.Fold,
			_playerManager.GetCurrentTurnPlayer().Id));
		
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

		int playerBet = player.Balance;
		
		CurrentPot += playerBet;
		player.RemoveFromBalance(playerBet);

		if (hand.Bet > CurrentBet)
			CurrentBet = hand.Bet;

		AdvanceTurn();
		
		RaiseDomainEvent(new PlayerTookActionDomainEvent(
			Id,
			playerId,
			PlayerActionType.AllIn,
			_playerManager.GetCurrentTurnPlayer().Id,
			playerBet));
		
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
				Cards: p.Id == requestingPlayerId
					? p.Hand?.Cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList()
					: null
			))
			.ToList();

		return new GameStateDto(
			CurrentPhase,
			CommunityCards: CommunityCards.Select(c => new CardDto(c.Suit, c.Rank)).ToList(),
			CurrentPot: CurrentPot,
			CurrentBet: CurrentBet,
			MinimumRaise: MinimumRaise,
			CurrentTurnPlayerId: _playerManager.GetCurrentTurnPlayer().Id,
			Players: players
		);
	}
	
	public PlayerInfoDto? GetPlayerDto(string playerId)
		=> _playerManager.GetPlayers()
			.FirstOrDefault(p => p.Id == playerId)?
			.ToDto();

	private void AdvanceTurn()
	{
		if (_playerManager.OnlyOneActivePlayer())
		{
			HandleShowdown();
		}
		else if (_playerManager.IsBettingRoundComplete(CurrentBet))
		{
			var cards = AdvancePhase();
			CommunityCards.AddRange(cards);
			
			var cardDtos = cards.Select(c => new CardDto(c.Suit, c.Rank)).ToList();

			RaiseDomainEvent(new GamePhaseUpdatedDomainEvent(
				Id,
				CurrentPhase,
				cardDtos));
			
			_playerManager.ResetHandsForNextRound();
			CurrentBet = 0;
			_playerManager.SetFirstActivePlayer();
		}
		else
		{
			_playerManager.GetNextActivePosition();
		}
	}

	private List<Card> AdvancePhase()
	{
		switch (CurrentPhase)
		{
			case GamePhase.PreFlop:
				CurrentPhase = GamePhase.Flop;
				return[
					_deck.Draw(),
					_deck.Draw(),
					_deck.Draw()];
			case GamePhase.Flop:
				CurrentPhase = GamePhase.Turn;
				return [_deck.Draw()];
			case GamePhase.Turn:
				CurrentPhase = GamePhase.River;
				return [_deck.Draw()];
			case GamePhase.River:
				HandleShowdown();
				break;
			default:
				throw new InvalidOperationException("Unknown phase.");
		}
		return new List<Card>();
	}
	
	private void HandleShowdown()
	{
		// TODO: Reset game.
		
		CurrentPhase = GamePhase.Showdown;
		
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

		var share = CurrentPot / winners.Count();

		foreach (var winner in winners)
		{
			winner.Player.AddToBalance(share);
		}

		RaiseDomainEvent(new ShowdownDomainEvent(
			Id, 
			winners.Select(w => w.Player.Id).ToList(),
			share));
	}
}
