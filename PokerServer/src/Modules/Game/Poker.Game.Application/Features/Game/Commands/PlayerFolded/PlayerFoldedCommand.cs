using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.PlayerFolded;

public sealed record PlayerFoldedCommand(
    string TableId,
    string PlayerId) : ICommand;
