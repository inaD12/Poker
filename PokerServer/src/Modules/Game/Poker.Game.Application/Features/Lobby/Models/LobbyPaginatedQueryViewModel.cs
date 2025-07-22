namespace Poker.Game.Application.Features.Lobby.Models;

public record LobbyPaginatedQueryViewModel(
    ICollection<LobbyQueryViewModel> Items,
    int Page,
    int PageSize,
    int TotalCount,
    bool HasNextPage,
    bool HasPreviousPage);