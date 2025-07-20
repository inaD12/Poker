namespace Poker.Users.Presentation.Features.Models.Responses;

public sealed record UserQueryResponse(
	string Id,
	string Email,
	string Username,
	int GamesPlayed,
	int GamesWon,
	decimal TotalEarnings);
