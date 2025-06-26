using Poker.Common.Domain.Results;
using Poker.Game.Domain.Enums;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;
	
public sealed class GameState
{
	public List<Card> CommunityCards { get; private set; }
	public int CurrentPot { get; private set; }
	public Dictionary<string, Hand> Hands { get; private set; }
	public List<string> PlayerOrder { get; private set; }
	public int CurrentTurnPlayerPosition { get; private set; }
	public int DealerPosition { get; private set; }
	public GamePhase Phase { get; private set; }
	public int CurrentBet { get; private set; }
	public int MinimumRaise { get; private set; }

	private GameState() { }

	private GameState(
		List<Card> communityCards,
		int currentPot,
		Dictionary<string, Hand> hands,
		List<string> playerOrder,
		int currentTurnPlayerPosition,
		int dealerPosition,
		GamePhase phase,
		int currentBet,
		int minimumRaise)
	{
		CommunityCards = communityCards;
		CurrentPot = currentPot;
		Hands = hands;
		PlayerOrder = playerOrder;
		CurrentTurnPlayerPosition = currentTurnPlayerPosition;
		DealerPosition = dealerPosition;
		Phase = phase;
		CurrentBet = currentBet;
		MinimumRaise = minimumRaise;
	}

	internal static Result<GameState> Create(
		List<string> playerOrder,
		int currentTurnPlayerPosition,
		int dealerPosition,
		int minimumRaise)
	{
		if (playerOrder is null || playerOrder.Count < 2)
			return Result<GameState>.Failure(ResponseList.TwoPlayersRequired);

		var gs =  new GameState(
			communityCards: new List<Card>(5),
			currentPot: 0,
			hands: new Dictionary<string, Hand>(),
			playerOrder: playerOrder,
			currentTurnPlayerPosition: currentTurnPlayerPosition,
			dealerPosition: dealerPosition,
			phase: GamePhase.PreFlop,
			currentBet: 0,
			minimumRaise: minimumRaise
		);

		return Result<GameState>.Success(gs);
	}

	internal void DealHands(List<Hand> hands)
	{
		if (Phase != GamePhase.PreFlop)
			throw new InvalidOperationException("Cannot deal hands outside PreFlop phase.");

		if (hands == null || hands.Count != PlayerOrder.Count)
			throw new ArgumentException("Hands must match the number of players.");


		var handDict = hands.ToDictionary(h => h.PlayerId);

		foreach (var playerId in PlayerOrder)
		{
			if (!handDict.ContainsKey(playerId))
				throw new ArgumentException($"No hand found for player {playerId}.");
		}

		Hands.Clear();
		Hands = handDict;
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
		CurrentTurnPlayerPosition = (CurrentTurnPlayerPosition + 1) % PlayerOrder.Count;
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
		foreach (var hand in Hands.Values)
		{
			if (!hand.IsFolded && !hand.IsAllIn)
				hand.ResetBet();
		}
		CurrentBet = 0;
	}

	internal void SetFirstActivePlayer()
	{
		for (int i = 0; i < PlayerOrder.Count; i++)
		{
			string playerId = PlayerOrder[i];
			if (!Hands[playerId].IsFolded && !Hands[playerId].IsAllIn)
			{
				CurrentTurnPlayerPosition = i;
				return;
			}
		}
	}
}

