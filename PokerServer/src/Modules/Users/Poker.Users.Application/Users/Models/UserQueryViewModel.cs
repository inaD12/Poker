namespace Poker.Users.Application.Users.Models;

public sealed record UserQueryViewModel(
    string Id,
    string Email,
    string Username,
    int GamesPlayed,
    int GamesWon,
    decimal TotalEarnings);