using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.Events;

public record PlayerTookActionDomainEvent (
    string GameId,
    string PlayerId,
    PlayerActionType Action,
    string NextPlayerId, 
    int? Amount = null) :IDomainEvent;