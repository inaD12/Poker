using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Abstractions.Interfaces;

namespace Poker.Game.Application.Features.Lobby.Queries.GetAllLobbiesPaged;

internal sealed class GetAllLobbiesPagedQueryHandler : IQueryHandler<GetAllLobbiesPagedQuery, LobbyPaginatedQueryViewModel>
{
    private readonly ILobbyRepository _lobbyRepository;
    private readonly IPokerMapper _pokerMapper;

    public GetAllLobbiesPagedQueryHandler(ILobbyRepository  lobbyRepository, IPokerMapper pokerMapper)
    {
        _lobbyRepository = lobbyRepository;
        _pokerMapper = pokerMapper;
    }

    public async Task<Result<LobbyPaginatedQueryViewModel>> Handle(GetAllLobbiesPagedQuery request, CancellationToken cancellationToken)
    {
        var lobbies = await _lobbyRepository.GetAllLobbiesPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        var lobbiesViewModel = _pokerMapper.Map<LobbyPaginatedQueryViewModel>(lobbies);
        return Result<LobbyPaginatedQueryViewModel>.Success(lobbiesViewModel);
    }
}