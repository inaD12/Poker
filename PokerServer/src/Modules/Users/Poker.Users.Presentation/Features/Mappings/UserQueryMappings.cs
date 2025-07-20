using AutoMapper;
using Poker.Users.Application.Users.Models;
using Poker.Users.Application.Users.Queries.GetUserById;
using Poker.Users.Presentation.Features.Models.Responses;

namespace Poker.Users.Presentation.Features.Mappings;

public class UserQueryMappings : Profile
{
	public UserQueryMappings()
	{
		CreateMap<string, GetUserByIdQuery>()
			.ConstructUsing(src => new(src));


		CreateMap<UserQueryViewModel, UserQueryResponse>();
	}
}
