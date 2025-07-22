using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Models;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Features.Lobby.Mappings;

public class LobbyQueryProfile : Profile
{
    public LobbyQueryProfile()
    {
        CreateMap<Domain.Entities.Lobby, LobbyQueryViewModel>();

        CreateMap<PagedList<Domain.Entities.Lobby>, LobbyPaginatedQueryViewModel>();
    }
}