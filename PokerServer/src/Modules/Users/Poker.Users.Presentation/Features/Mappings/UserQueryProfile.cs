using AutoMapper;
using Poker.Common.Domain.Dtos;
using Poker.Users.Application.Users.Models;
using Poker.Users.Application.Users.Queries.GetUserById;
using Poker.Users.Presentation.Features.Models.Responses;

namespace Poker.Users.Presentation.Features.Mappings;

public class UserQueryProfile : Profile
{
	public UserQueryProfile()
	{
		CreateMap<string, GetUserByIdQuery>()
			.ConstructUsing(src => new(src));

		CreateMap<UserQueryViewModel, UserDataDto>()
			.ConstructUsing(src => new(
				src.Id,
				src.Username));
		
		CreateMap<UserQueryViewModel, UserQueryResponse>();
	}
}
