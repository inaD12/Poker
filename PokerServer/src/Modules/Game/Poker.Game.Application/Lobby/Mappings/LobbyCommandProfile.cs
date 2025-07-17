using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Game.Application.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Lobby.Mappings;

public class LobbyCommandProfile: Profile
{
    public LobbyCommandProfile()
    {
        CreateMap<UserDataDto, Player>();
        CreateMap<string, LobbyCommandViewModel>();
    }
}