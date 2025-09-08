using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record PlayerPlayedHandDomainEvent(
    string Id,
    bool Won = false,
    decimal Earnings = 0) : IDomainEvent;