using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;

namespace Poker.Game.Domain.Events;

public record PlayerPlayedGameDomainEvent(
    string Id,
    bool Won = false,
    decimal Earnings = 0) : IDomainEvent;