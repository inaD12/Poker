using FluentValidation;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Game.Commands.PlayerLeave;

internal class PlayerLeaveCommandValidator : AbstractValidator<PlayerLeaveCommand>
{
    public PlayerLeaveCommandValidator()
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