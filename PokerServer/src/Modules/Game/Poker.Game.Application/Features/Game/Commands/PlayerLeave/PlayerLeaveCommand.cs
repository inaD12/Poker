using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.PlayerLeave;

public sealed record PlayerLeaveCommand(
    string TableId,
    string PlayerId) : ICommand;