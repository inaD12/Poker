using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record PlayerKickedDomainEvent(
    string TableId,
    string PlayerId) : IDomainEvent;