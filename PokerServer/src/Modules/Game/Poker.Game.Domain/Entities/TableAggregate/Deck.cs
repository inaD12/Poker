using Poker.Common.Domain.Enums;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Deck
{
	private readonly Stack<Card> _cards;

	private Deck(IEnumerable<Card> cards)
	{
		_cards = new Stack<Card>(cards);
	}

	public static Deck CreateShuffled()
	{
		var cards = Enum.GetValues<CardSuit>()
			.SelectMany(suit => Enum.GetValues<CardRank>().Select(rank => Card.Create(suit, rank)))
			.ToList();

		var rng = new Random();
		cards = cards.OrderBy(_ => rng.Next()).ToList();

		return new Deck(cards);
	}

	public Card Draw()
	{
		if (_cards.Count == 0)
			throw new InvalidOperationException("The deck is empty.");

		return _cards.Pop();
	}

	public int Count => _cards.Count;
}