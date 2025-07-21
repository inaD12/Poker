using Poker.Common.Domain.Enums;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Deck
{
    private Deck(IEnumerable<Card> cards)
    {
        Cards = new Stack<Card>(cards);
    }

    public Stack<Card> Cards { get; }

    public int Count => Cards.Count;

    public static Deck CreateShuffled()
    {
        var cards = Enum.GetValues<CardSuit>()
            .SelectMany(suit => Enum.GetValues<CardRank>().Select(rank => Card.Create(suit, rank)))
            .ToList();

        var rng = new Random();
        cards = cards.OrderBy(_ => rng.Next()).ToList();

        return new Deck(cards);
    }

    internal Card Draw()
    {
        if (Cards.Count == 0)
            throw new InvalidOperationException("The deck is empty.");

        return Cards.Pop();
    }
}