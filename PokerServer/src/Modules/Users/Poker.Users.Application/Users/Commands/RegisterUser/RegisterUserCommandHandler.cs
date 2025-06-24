using Poker.Common.Application.Abstractions;
using Poker.Common.Domain.Abstractions;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Abstractions.Auth;
using Poker.Users.Domain.Abstractions.Auth.Models;
using Poker.Users.Domain.Entities;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;
	private readonly IPokerMapper _hamsMapper;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterUserCommandHandler(IPasswordManager passwordManager, IPokerMapper hamsMapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_passwordManager = passwordManager;
		_hamsMapper = hamsMapper;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result<UserCommandViewModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var emailUser = await _userRepository.GetByEmailAsync(request.Email);
		if (emailUser != null)
			return Result<UserCommandViewModel>.Failure(ResponseList.EmailTaken);

		PasswordHashResult passwordHashResult = _passwordManager.HashPassword(request.Password);
		var user = User.Create(request.Email, passwordHashResult.PasswordHash, passwordHashResult.Salt, request.Username);
		await _userRepository.AddAsync(user);
		await _unitOfWork.SaveChangesAsync();

		var userCommandViewModel = _hamsMapper.Map<UserCommandViewModel>(user);
		return Result<UserCommandViewModel>.Success(userCommandViewModel);
	}
}
