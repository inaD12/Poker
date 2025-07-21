using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record ShowdownDomainEvent(
    string TableId,
    List<string> WinnerPlayerIds,
    int WinningsEach) : IDomainEvent;