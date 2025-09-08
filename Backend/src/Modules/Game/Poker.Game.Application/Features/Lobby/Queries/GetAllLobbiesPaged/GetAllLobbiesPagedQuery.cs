using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Application.Features.Lobby.Queries.GetAllLobbiesPaged;

public sealed record GetAllLobbiesPagedQuery(
    int PageNumber,
    int PageSize,
    CancellationToken CancellationToken) : IQuery<LobbyPaginatedQueryViewModel>;