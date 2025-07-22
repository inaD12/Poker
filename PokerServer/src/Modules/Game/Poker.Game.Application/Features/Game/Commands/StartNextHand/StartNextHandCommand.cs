using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.StartNextHand;

public record StartNextHandCommand(
    string TableId,
    string PlayerId) : ICommand;