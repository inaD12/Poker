using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;

namespace Poker.Users.Presentation.Features.Services;

public interface IUserService
{
    Task<Result<UserDataDto>> GetUserDataById(string id, CancellationToken cancellationToken);
    Task<Result> UserPlayedGame(string id, bool won = false, decimal earnings = 0, CancellationToken cancellationToken = default);
}