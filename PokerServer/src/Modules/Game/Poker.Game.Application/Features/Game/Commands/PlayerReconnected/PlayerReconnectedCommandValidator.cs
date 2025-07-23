using FluentValidation;
using Poker.Game.Application.Features.Game.Commands.PlayerDisconnected;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Game.Commands.PlayerReconnected;

internal class PlayerReconnectedCommandValidator : AbstractValidator<PlayerReconnectedCommand>
{
    public PlayerReconnectedCommandValidator()
    {
        RuleFor(x => x.TableId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);

        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
    }
}