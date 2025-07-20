using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Presentation.Abstractions;
using Poker.Common.Presentation.Endpoints;
using Poker.Common.Presentation.Helpers;
using Poker.Users.Application.Users.Commands.DeleteUser;
using Poker.Users.Application.Users.Commands.LoginUser;
using Poker.Users.Application.Users.Commands.RegisterUser;
using Poker.Users.Application.Users.Commands.UpdateUser;
using Poker.Users.Application.Users.Queries.GetUserById;
using Poker.Users.Presentation.Features.Models.Requests;
using Poker.Users.Presentation.Features.Models.Responses;

namespace Poker.Users.Presentation.Features.Endpoints;

internal class UserEndpoints : IEndpoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/users");

		group.MapPost("login", Login)
			.Produces<LoginUserResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPost("register", Register)
			.Produces<UserCommandResponse>(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPut("update-current", UpdateCurrent)
			.Produces<UserCommandResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization();

		group.MapPut("update/{id}", Update)
			.Produces<UserCommandResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);

		group.MapGet("get/{id}", GetById)
			.Produces<UserQueryResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapDelete("delete-current", DeleteCurrent)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization();

		group.MapDelete("delete/{id}", DeleteById)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();
	}

	public async Task<IResult> Login(
		[FromBody] LoginUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IPokerMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<LoginUserCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var loginUserResponse = mapper.Map<LoginUserResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, loginUserResponse);
	}

	public async Task<IResult> Register(
		[FromBody] RegisterUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IPokerMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<RegisterUserCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> UpdateCurrent(
		[FromBody] UpdateCurrentUserRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		[FromServices] IPokerMapper mapper,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = mapper.Map<UpdateUserCommand>((request, userId));
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> Update(
		[FromRoute] string id,
		[FromBody] UpdateUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IPokerMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<UpdateUserCommand>((request, id));
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> GetById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		[FromServices] IPokerMapper mapper,
		CancellationToken cancellationToken)
	{
		var query = mapper.Map<GetUserByIdQuery>(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = mapper.Map<UserQueryResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}

	public async Task<IResult> DeleteById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new DeleteUserCommand(id);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> DeleteCurrent(
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = new DeleteUserCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}