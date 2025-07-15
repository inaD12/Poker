using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Game.Application.Game.Models;
using Poker.Game.Domain.Entities;

namespace Poker.Game.Application.Game.Mappings;

public class GameCommandProfile: Profile
{
    public GameCommandProfile()
    {
        CreateMap<UserDataDto, Player>();
        CreateMap<string, GameCommandViewModel>();
    }
}