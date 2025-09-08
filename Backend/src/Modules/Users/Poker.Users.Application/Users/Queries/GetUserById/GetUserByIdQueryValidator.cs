using FluentValidation;
using Poker.Users.Domain.Utilities;

namespace Poker.Users.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty()
            .MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);
    }
}