using FluentValidation;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Game.Commands.GameStart;

public class GameStartCommandValidator: AbstractValidator<GameStartCommand>
{
    public GameStartCommandValidator()
    {
        RuleFor(x => x.PlayerIds)
            .NotNull().WithMessage("Player IDs are required.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate player IDs are not allowed.")
            .Must(ids => ids.Count is >= 2 and <= 6)
                .WithMessage("The number of players must be between 2 and 6.");

        RuleForEach(x => x.PlayerIds)
            .NotEmpty()
            .MinimumLength(GameBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(GameBusinessConfiguration.ID_MAX_LENGTH);
    }
}