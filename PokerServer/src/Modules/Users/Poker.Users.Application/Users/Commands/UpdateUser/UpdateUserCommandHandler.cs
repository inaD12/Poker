using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPokerMapper _mapper;
	public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IPokerMapper mapper, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_userRepository = userRepository;
	}

	public async Task<Result<UserCommandViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
		if (user == null)
			return Result<UserCommandViewModel>.Failure(ResponseList.UserNotFound);

		var result = user.UpdateUsername(request.NewUsername);
		if (result.IsFailure)
			return Result<UserCommandViewModel>.Failure(result.Response!);

		_userRepository.Update(user);

		await _unitOfWork.SaveChangesAsync(cancellationToken);
		var userCommandViewModel = _mapper.Map<UserCommandViewModel>(user);
		return Result<UserCommandViewModel>.Success(userCommandViewModel);
	}
}
