using FluentValidation;
using Poker.Game.Application.Features.Game.Commands.PlayerLeave;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Game.Commands.KickPlayer;

internal class KickPlayerCommandValidator : AbstractValidator<KickPlayerCommand>
{
    public KickPlayerCommandValidator()
    {
        RuleFor(x => x.TableId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);

        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
        
        RuleFor(x => x.CallingPlayerId)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
    }
}