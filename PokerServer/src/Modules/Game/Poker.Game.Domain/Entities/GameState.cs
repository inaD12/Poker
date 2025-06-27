using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public sealed class GameState
{
	public List<Card> CommunityCards { get; private set; }
	public int CurrentPot { get; private set; }
	public List<Player> Players { get; private set; }
	public Dictionary<string, Player> PlayerDictionary { get; private set; }
	public int CurrentTurnPlayerPosition { get; private set; }
	public int DealerPosition { get; private set; }
	public GamePhase Phase { get; private set; }
	public int CurrentBet { get; private set; }
	public int MinimumRaise { get; private set; }

	private GameState() { }

	private GameState(
		List<Card> communityCards,
		int currentPot,
		List<Player> players,
		int currentTurnPlayerPosition,
		int dealerPosition,
		GamePhase phase,
		int currentBet,
		int minimumRaise)
	{
		CommunityCards = communityCards;
		CurrentPot = currentPot;
		Players = players;
		PlayerDictionary = players.ToDictionary(p => p.Id);
		CurrentTurnPlayerPosition = currentTurnPlayerPosition;
		DealerPosition = dealerPosition;
		Phase = phase;
		CurrentBet = currentBet;
		MinimumRaise = minimumRaise;
	}

	internal static Result<GameState> Create(
		List<Player> players,
		int currentTurnPlayerPosition,
		int dealerPosition,
		int minimumRaise)
	{
		if (players.Count < 2)
			return Result<GameState>.Failure(ResponseList.TwoPlayersRequired);

		var gs = new GameState(
			communityCards: new List<Card>(5),
			currentPot: 0,
			players: players,
			currentTurnPlayerPosition: currentTurnPlayerPosition,
			dealerPosition: dealerPosition,
			phase: GamePhase.PreFlop,
			currentBet: 0,
			minimumRaise: minimumRaise
		);

		return Result<GameState>.Success(gs);
	}

	internal void Flop(List<Card> flopCards)
	{
		if (Phase != GamePhase.PreFlop)
			throw new InvalidOperationException("Flop can only be dealt after PreFlop phase.");

		if (flopCards.Count != 3)
			throw new ArgumentException("Flop must consist of exactly 3 cards.");

		CommunityCards.AddRange(flopCards);
		Phase = GamePhase.Flop;
	}

	internal void Turn(Card turnCard)
	{
		if (Phase != GamePhase.Flop)
			throw new InvalidOperationException("Turn can only be dealt after Flop phase.");

		CommunityCards.Add(turnCard);
		Phase = GamePhase.Turn;
	}

	internal void River(Card riverCard)
	{
		if (Phase != GamePhase.Turn)
			throw new InvalidOperationException("River can only be dealt after Turn phase.");

		CommunityCards.Add(riverCard);
		Phase = GamePhase.River;
	}

	internal void NextPlayer()
	{
		CurrentTurnPlayerPosition = (CurrentTurnPlayerPosition + 1) % Players.Count;
	}

	internal void AddToPot(int amount)
	{
		if (amount <= 0)
			throw new ArgumentException("Amount to add to pot must be greater than zero.");
		CurrentPot += amount;
	}

	internal void UpdateCurrentBet(int amount)
	{
		if (amount < 0)
			throw new ArgumentException("Current bet cannot be negative.");

		CurrentBet = amount;
	}
	internal void ResetBetsForNextRound()
	{
		foreach (var player in Players)
		{
			if (player.Hand != null)
				player.Hand.ResetBet();
		}
		CurrentBet = 0;
	}

	internal void SetFirstActivePlayer()
	{
		for (int i = 0; i < Players.Count; i++)
		{
			var player = Players[i];
			if (player.Hand == null || player.Hand.IsFolded || player.Hand.IsAllIn)
				continue;

			CurrentTurnPlayerPosition = i;
			return;
		}
	}
}

