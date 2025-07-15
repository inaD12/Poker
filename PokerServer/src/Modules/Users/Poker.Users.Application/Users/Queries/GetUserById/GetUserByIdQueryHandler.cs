using Poker.Common.Application.Abstractions;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Responses;

namespace Poker.Users.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserQueryViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IPokerMapper _mapper;

	public GetUserByIdQueryHandler(IUserRepository userRepository, IPokerMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<Result<UserQueryViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		if (user == null)
			return Result<UserQueryViewModel>.Failure(ResponseList.UserNotFound);

		var userQueryViewModel = _mapper.Map<UserQueryViewModel>(user);
		return Result<UserQueryViewModel>.Success(userQueryViewModel);
	}
}
