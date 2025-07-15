using Poker.Common.Application.Abstractions;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Abstractions.Auth;
using Poker.Users.Domain.Abstractions.Auth.Models;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Commands.LoginUser;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;
	private readonly ITokenFactory _tokenManager;
	private readonly IPokerMapper _mapper;

	public LoginUserCommandHandler(IPasswordManager passwordManager, ITokenFactory tokenManager, IPokerMapper mapper, IUserRepository userRepository)
	{
		_passwordManager = passwordManager;
		_tokenManager = tokenManager;
		_mapper = mapper;
		_userRepository = userRepository;
	}

	public async Task<Result<LoginUserCommandViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByEmailAsync(request.Email);

		if (user == null)
			return Result<LoginUserCommandViewModel>.Failure(ResponseList.UserNotFound);

		if (!_passwordManager.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
			return Result<LoginUserCommandViewModel>.Failure(ResponseList.IncorrectPassword);

		TokenResult token = _tokenManager.CreateToken(user.Id);
		var loginUserCommandViewModel = _mapper.Map<LoginUserCommandViewModel>(token);
		return Result<LoginUserCommandViewModel>.Success(loginUserCommandViewModel);
	}
}
