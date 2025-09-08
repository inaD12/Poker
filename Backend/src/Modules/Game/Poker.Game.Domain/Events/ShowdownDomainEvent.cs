using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Dtos;

namespace Poker.Game.Domain.Events;

public record ShowdownDomainEvent(
    string TableId,
    List<string> WinnerPlayerIds,
    int WinningsEach,
    List<PlayerStateDto> players) : IDomainEvent;