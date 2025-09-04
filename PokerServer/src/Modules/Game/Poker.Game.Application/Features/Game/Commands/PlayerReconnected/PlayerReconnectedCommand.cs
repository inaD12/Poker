using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.PlayerReconnected;

public sealed record PlayerReconnectedCommand(
    string TableId,
    string PlayerId) : ICommand;