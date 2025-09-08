using AutoMapper;
using Poker.Users.Application.Users.Commands.LoginUser;
using Poker.Users.Application.Users.Commands.RegisterUser;
using Poker.Users.Application.Users.Commands.UpdateUser;
using Poker.Users.Application.Users.Models;
using Poker.Users.Presentation.Features.Models.Requests;
using Poker.Users.Presentation.Features.Models.Responses;

namespace Poker.Users.Presentation.Features.Mappings;

public class UserCommandProfile : Profile
{
    public UserCommandProfile()
    {
        CreateMap<LoginUserRequest, LoginUserCommand>();

        CreateMap<RegisterUserRequest, RegisterUserCommand>();

        CreateMap<(UpdateCurrentUserRequest, string id), UpdateUserCommand>()
            .ConstructUsing(src => new UpdateUserCommand(
                src.Item2,
                src.Item1.NewUsername));

        CreateMap<(UpdateUserRequest, string id), UpdateUserCommand>()
            .ConstructUsing(src => new UpdateUserCommand(
                src.Item2,
                src.Item1.NewUsername));

        CreateMap<UserCommandViewModel, UserCommandResponse>();
    }
}