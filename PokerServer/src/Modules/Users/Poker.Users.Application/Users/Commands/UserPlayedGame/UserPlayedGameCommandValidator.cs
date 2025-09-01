using FluentValidation;
using Poker.Users.Domain.Utilities;

namespace Poker.Users.Application.Users.Commands.UserPlayedGame;

internal class UserPlayedGameCommandValidator : AbstractValidator<UserPlayedGameCommand>
{
    public UserPlayedGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);

        RuleFor(x => x.Earnings)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Earnings cannot be negative.");

        RuleFor(x => x)
            .Must(cmd => cmd.Won || cmd.Earnings == 0)
            .WithMessage("Earnings must be 0 if the game was not won.");
    }
}