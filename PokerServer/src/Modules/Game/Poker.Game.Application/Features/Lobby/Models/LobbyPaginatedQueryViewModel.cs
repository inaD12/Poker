namespace Poker.Game.Application.Features.Lobby.Models;

public record LobbyPaginatedQueryViewModel(
    ICollection<LobbyViewModel> Items,
    int Page,
    int PageSize,
    int TotalCount,
    bool HasNextPage,
    bool HasPreviousPage);