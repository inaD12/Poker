using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record PlayerReconnectedDomainEvent(
    string TableId,
    string PlayerId) : IDomainEvent;