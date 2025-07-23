using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record PlayerDisconnectedDomainEvent(
    string TableId,
    string PlayerId) : IDomainEvent;