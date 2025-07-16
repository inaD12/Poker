using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Game.Commands.PlayerPlacedBet;

public sealed record PlayerPlacedBetCommand(
    string TableId,
    string PlayerId,
    int Amount) : ICommand;
