using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.PlayerDisconnected;

public sealed record PlayerDisconnectedCommand(
    string TableId,
    string PlayerId) : ICommand;