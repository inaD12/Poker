using AutoMapper;
using Poker.Common.Domain.Models;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Lobby.Mappings;

public class LobbyQueryProfile : Profile
{
    public LobbyQueryProfile()
    {
        CreateMap<Domain.Entities.Lobby, LobbyQueryViewModel>()
            .ConstructUsing(src => new LobbyQueryViewModel(
                src.Id,
                src.Name,
                src.HostingPlayerName,
                src.CreatedAt,
                src.Players.Select(p => p.ToDto()).ToList(),
                src.IsFull,
                src.IsReadyToStart));


        CreateMap<PagedList<Domain.Entities.Lobby>, LobbyPaginatedQueryViewModel>();
    }
}