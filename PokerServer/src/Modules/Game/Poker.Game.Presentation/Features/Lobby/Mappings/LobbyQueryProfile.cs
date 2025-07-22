using AutoMapper;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Application.Features.Lobby.Queries.GetAllLobbiesPaged;
using Poker.Game.Presentation.Features.Lobby.Models;

namespace Poker.Game.Presentation.Features.Lobby.Mappings;

public class LobbyQueryProfile : Profile
{
    public LobbyQueryProfile()
    {
        // CreateMap<string, GetUserByIdQuery>()
        //     .ConstructUsing(src => new GetUserByIdQuery(src));

        CreateMap<GetAllLobbiesRequest, GetAllLobbiesPagedQuery>();

        CreateMap<LobbyQueryViewModel, LobbyQueryResponse>();

        CreateMap<LobbyPaginatedQueryViewModel, LobbyPaginatedQueryResponse>();
    }
}