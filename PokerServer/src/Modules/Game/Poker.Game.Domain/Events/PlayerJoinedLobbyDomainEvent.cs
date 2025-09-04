using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Domain.Events;

public record PlayerJoinedLobbyDomainEvent(
    string TableId,
    Player Player) : IDomainEvent;