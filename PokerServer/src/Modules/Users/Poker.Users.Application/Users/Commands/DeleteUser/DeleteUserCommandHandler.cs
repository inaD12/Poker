using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		if (user == null)
			return Result.Failure(ResponseList.UserNotFound);

		_userRepository.Delete(user);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
