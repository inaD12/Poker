using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Commands.UserPlayedGame;

internal sealed class UserPlayedHandCommandHandler : ICommandHandler<UserPlayedHandCommand, UserCommandViewModel>
{
    private readonly IPokerMapper _mapper;
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IUserRepository _userRepository;

    public UserPlayedHandCommandHandler(IUsersUnitOfWork usersUnitOfWork, IPokerMapper mapper, IUserRepository userRepository)
    {
        _usersUnitOfWork = usersUnitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<Result<UserCommandViewModel>> Handle(UserPlayedHandCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            return Result<UserCommandViewModel>.Failure(ResponseList.UserNotFound);

        user.PlayedHand(request.Won, request.Earnings);

        _userRepository.Update(user);

        await _usersUnitOfWork.SaveChangesAsync(cancellationToken);
        var userCommandViewModel = _mapper.Map<UserCommandViewModel>(user);
        return Result<UserCommandViewModel>.Success(userCommandViewModel);
    }
}