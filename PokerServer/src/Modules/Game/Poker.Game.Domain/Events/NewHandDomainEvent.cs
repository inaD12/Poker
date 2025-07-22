using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record NewHandDomainEvent(
    string TableId) : IDomainEvent;