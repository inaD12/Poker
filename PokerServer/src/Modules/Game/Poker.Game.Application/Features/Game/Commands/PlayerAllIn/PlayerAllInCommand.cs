using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.PlayerAllIn;

public sealed record PlayerAllInCommand(
    string TableId,
    string PlayerId) : ICommand;
