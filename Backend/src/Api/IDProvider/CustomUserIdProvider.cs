using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Poker.Common.Utilities;

namespace PokerServer.IDProvider;

public class CustomUserIdProvider : IUserIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomUserIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId(HubConnectionContext connection)
    {
        var user = _httpContextAccessor.HttpContext?.User!;
        return user.FindFirstValue(AppClaims.Id)!;
    }
}