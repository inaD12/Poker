using FluentValidation;
using Poker.Users.Domain.Utilities;

namespace Poker.Users.Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.NewUsername)
			.MinimumLength(UsersBusinessConfiguration.USERNAME_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.USERNAME_MAX_LENGTH)
			.EmailAddress();
	}
}
