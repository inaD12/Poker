using Newtonsoft.Json;
using Poker.Common.Domain.Enums;

namespace Poker.Game.Domain.Entities.TableAggregate;

public sealed class Card
{
    private Card()
    {
    }

    [JsonConstructor]
    private Card(CardSuit suit, CardRank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public CardSuit Suit { get; private set; }
    public CardRank Rank { get; private set; }

    public static Card Create(CardSuit suit, CardRank rank)
    {
        return new Card(suit, rank);
    }
}