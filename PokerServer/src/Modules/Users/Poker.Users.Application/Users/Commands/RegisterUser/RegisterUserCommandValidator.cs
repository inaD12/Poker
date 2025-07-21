using FluentValidation;
using Poker.Users.Domain.Utilities;

namespace Poker.Users.Application.Users.Commands.RegisterUser;

internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.EMAIL_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.EMAIL_MAX_LENGTH)
			.EmailAddress();

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);

		RuleFor(x => x.Username)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.USERNAME_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.USERNAME_MAX_LENGTH);
	}
}
