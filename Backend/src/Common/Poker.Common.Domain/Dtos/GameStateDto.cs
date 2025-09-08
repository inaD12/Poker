using Poker.Common.Domain.Enums;

namespace Poker.Common.Domain.Dtos;

public record GameStateDto(
    GamePhase Phase,
    IReadOnlyList<CardDto> CommunityCards,
    int CurrentPot,
    int CurrentBet,
    int MinimumRaise,
    string? CurrentTurnPlayerId,
    string DealerPlayerId,
    string HostingPlayerId,
    IReadOnlyList<PlayerStateDto> Players
);