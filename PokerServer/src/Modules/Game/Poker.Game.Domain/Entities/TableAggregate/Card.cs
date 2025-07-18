using Poker.Common.Domain.Enums;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Card
{
	public CardSuit Suit { get; private set; }
	public CardRank Rank { get; private set; }

	private Card() { }

	private Card(CardSuit suit, CardRank rank)
	{
		Suit = suit;
		Rank = rank;
	}

	public static Card Create(CardSuit suit, CardRank rank)
	{
		return new Card(suit, rank);
	}
}
