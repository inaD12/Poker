using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;

namespace Poker.Users.Presentation.Features.Services;

public interface IUserService
{
    Task<Result<UserDataDto>> GetUserDataById(string id, CancellationToken cancellationToken); 
}