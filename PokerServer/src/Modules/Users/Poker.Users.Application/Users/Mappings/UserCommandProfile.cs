using AutoMapper;
using Poker.Users.Application.Users.Models;
using Poker.Users.Domain.Abstractions.Auth.Models;
using Poker.Users.Domain.Entities;

namespace Poker.Users.Application.Users.Mappings;

public class UserCommandProfile : Profile
{
	public UserCommandProfile()
	{
		CreateMap<TokenResult, LoginUserCommandViewModel>();

		CreateMap<User, UserCommandViewModel>();
	}
}