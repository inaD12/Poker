using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Presentation.Features.Lobby.Models;

public record LobbyPaginatedQueryResponse(
    ICollection<LobbyQueryViewModel> Items,
    int Page,
    int PageSize,
    int TotalCount,
    bool HasNextPage,
    bool HasPreviousPage);