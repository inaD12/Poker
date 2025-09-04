using FluentValidation;
using Poker.Game.Application.Features.Game.Commands.PlayerAllIn;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Game.Commands.StartNextHand;

internal class StartNextHandCommandValidator : AbstractValidator<PlayerAllInCommand>
{
    public StartNextHandCommandValidator()
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