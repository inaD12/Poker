using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.KickPlayer;

public sealed record KickPlayerCommand(
    string TableId,
    string PlayerId,
    string CallingPlayerId) : ICommand;