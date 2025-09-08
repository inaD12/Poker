using Poker.Common.Domain.Enums;

namespace Poker.Common.Domain.Dtos;

public record CardDto(CardSuit Suit, CardRank Rank);