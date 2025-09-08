using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Game.Domain.Events;

public record PlayerLeftLobbyDomainEvent(
    string TableId,
    string PlayerId) : IDomainEvent;