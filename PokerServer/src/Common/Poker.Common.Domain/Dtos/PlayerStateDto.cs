namespace Poker.Common.Domain.Dtos;

public record PlayerStateDto(
    string Id,
    string Username,
    int Balance,
    bool IsFolded,
    bool IsAllIn,
    int CurrentBet,
    bool IsCurrentTurn,
    bool isDisconnected,
    bool isSelf,
    IReadOnlyList<CardDto>? Cards
);