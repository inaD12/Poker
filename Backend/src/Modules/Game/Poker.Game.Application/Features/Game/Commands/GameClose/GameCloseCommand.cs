using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Game.Commands.GameClose;

public record GameCloseCommand(
    string TableId,
    string PlayerId) : ICommand;