using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Game.Commands.PlayerChecked;

public sealed record PlayerCheckedCommand(
    string TableId,
    string PlayerId) : ICommand;
