using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record NewHostDomainEvent(
    string PlayerId) : IDomainEvent;