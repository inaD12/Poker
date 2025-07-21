using FluentValidation;
using Poker.Users.Domain.Utilities;

namespace Poker.Users.Application.Users.Commands.LoginUser;

internal class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserCommandValidator()
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
	}
}
