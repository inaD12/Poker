using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Models;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Utilities;

namespace Poker.Game.Application.Features.Lobby.Mappings;

public class LobbyQueryProfile : Profile
{
    public LobbyQueryProfile()
    {
        CreateMap<Player, PlayerInfoDto>();

        CreateMap<Domain.Entities.Lobby, LobbyViewModel>()
            .ConstructUsing((src, ctx) => new LobbyViewModel(
                src.Id,
                src.Name,
                src.HostingPlayerName,
                src.CreatedAt,
                ctx.Mapper.Map<List<PlayerInfoDto>>(src.Players),
                src.IsFull,
                src.IsReadyToStart));

        CreateMap<PagedList<Domain.Entities.Lobby>, LobbyPaginatedQueryViewModel>();
    }
}