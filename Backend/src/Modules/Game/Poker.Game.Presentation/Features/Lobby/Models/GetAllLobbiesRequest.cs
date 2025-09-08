namespace Poker.Game.Presentation.Features.Lobby.Models;

public record GetAllLobbiesRequest(
    CancellationToken CancellationToken,
    int PageNumber = 1,
    int PageSize = 10 );
