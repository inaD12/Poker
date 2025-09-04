using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUsersUnitOfWork usersUnitOfWork, IUserRepository userRepository)
    {
        _usersUnitOfWork = usersUnitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            return Result.Failure(ResponseList.UserNotFound);

        _userRepository.Delete(user);
        await _usersUnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}