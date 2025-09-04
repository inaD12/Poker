using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Game.Application.Features.Game.Models;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Features.Game.Mappings;

public class GameCommandProfile : Profile
{
    public GameCommandProfile()
    {
        CreateMap<UserDataDto, Player>();
        CreateMap<string, GameCommandViewModel>()
            .ConstructUsing(src => new GameCommandViewModel(src));
    }
}