using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;

namespace Poker.Common.Infrastructure.Abstractions;

public interface IUserService
{
    Task<Result<List<UserDataDto>>> GetUserDataByIds(List<string> ids); 
}