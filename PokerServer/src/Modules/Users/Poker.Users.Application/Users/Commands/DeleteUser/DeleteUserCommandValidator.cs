using FluentValidation;
using Poker.Users.Domain.Utilities;

namespace Poker.Users.Application.Users.Commands.DeleteUser;

internal class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
	public DeleteUserCommandValidator()
	{
		RuleFor(x => x.Id)
				.NotEmpty()
				.MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
				.MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);
	}
}