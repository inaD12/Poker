using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Users.Application.Users.Models;

namespace Poker.Users.Presentation.Features.Mappings;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<UserQueryViewModel, UserDataDto>()
			.ConstructUsing(src => new(
				src.Id,
				src.Username));
	}
}