using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;

namespace Poker.Game.Domain.Events;

public record GamePhaseUpdatedDomainEvent(
    string TableId,
    GamePhase Phase,
    List<CardDto> Cards) : IDomainEvent;