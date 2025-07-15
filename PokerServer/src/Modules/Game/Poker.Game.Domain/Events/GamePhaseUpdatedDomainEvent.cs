using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.Events;

public record GamePhaseUpdatedDomainEvent(
    string GameId, 
    GamePhase Phase, 
    List<Card> Cards): IDomainEvent;