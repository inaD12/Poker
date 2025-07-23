namespace Poker.Common.Domain.Dtos;

public record PlayerStateDto(
    string Id,
    int Balance,
    bool IsFolded,
    bool IsAllIn,
    int CurrentBet,
    bool IsCurrentTurn,
    bool isDisconnected,
    IReadOnlyList<CardDto>? Cards
);