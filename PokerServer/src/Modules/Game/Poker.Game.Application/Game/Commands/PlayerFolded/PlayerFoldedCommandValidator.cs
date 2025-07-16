using FluentValidation;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Game.Commands.PlayerFolded;

public class PlayerFoldedCommandValidator: AbstractValidator<PlayerFoldedCommand>
{
    public PlayerFoldedCommandValidator()
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