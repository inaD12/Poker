using AutoMapper;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Entities;

namespace Poker.Users.Application.Users.Mappings;

public class UserQueryProfile : Profile
{
    public UserQueryProfile()
    {
        CreateMap<User, UserQueryViewModel>();
    }
}