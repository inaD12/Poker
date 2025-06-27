using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.Services;

public class PhaseManager
{
	public GamePhase CurrentPhase { get; private set; }

	public PhaseManager(GamePhase startingPhase)
	{
		CurrentPhase = startingPhase;
	}

	public void AdvancePhase(List<Card> communityCards, Deck deck ,Action handleShowdown)
	{
		switch (CurrentPhase)
		{
			case GamePhase.PreFlop:
				DealFlop(communityCards, deck);
				break;
			case GamePhase.Flop:
				DealTurn(communityCards, deck);
				break;
			case GamePhase.Turn:
				DealRiver(communityCards, deck);
				break;
			case GamePhase.River:
				handleShowdown();
				break;
			default:
				throw new InvalidOperationException("Unknown phase.");
		}
	}

	private void DealFlop(List<Card> communityCards, Deck deck)
	{
		if (CurrentPhase != GamePhase.PreFlop)
			throw new InvalidOperationException("Flop can only be dealt after PreFlop.");

		var flopCards = new List<Card>
		{
			deck.Draw(),
			deck.Draw(),
			deck.Draw()
		};

		communityCards.AddRange(flopCards);
		CurrentPhase = GamePhase.Flop;
	}

	private void DealTurn(List<Card> communityCards, Deck deck)
	{
		if (CurrentPhase != GamePhase.Flop)
			throw new InvalidOperationException("Turn can only be dealt after Flop.");

		communityCards.Add(deck.Draw());
		CurrentPhase = GamePhase.Turn;
	}

	private void DealRiver(List<Card> communityCards, Deck deck)
	{
		if (CurrentPhase != GamePhase.Turn)
			throw new InvalidOperationException("River can only be dealt after Turn.");

		communityCards.Add(deck.Draw());
		CurrentPhase = GamePhase.River;
	}
}
