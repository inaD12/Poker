using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Features.Lobby.Mappings;

public class LobbyCommandProfile : Profile
{
    public LobbyCommandProfile()
    {
        CreateMap<UserDataDto, Player>();
        CreateMap<string, LobbyCommandViewModel>()
            .ConstructUsing(src => new LobbyCommandViewModel(src));

    }
}