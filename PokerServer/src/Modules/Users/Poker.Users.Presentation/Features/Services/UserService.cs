using MediatR;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;
using Poker.Users.Application.Users.Queries.GetUserById;

namespace Poker.Users.Presentation.Features.Services;

internal class UserService : IUserService
{
    private readonly IPokerMapper _mapper;
    private readonly ISender _sender;

    public UserService(ISender sender, IPokerMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public async Task<Result<UserDataDto>> GetUserDataById(string id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);
        if (result.IsFailure)
            return Result<UserDataDto>.Failure(result.Response);

        var userData = _mapper.Map<UserDataDto>(result.Value!);
        return Result<UserDataDto>.Success(userData);
    }
}