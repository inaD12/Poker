namespace Poker.Users.Application.Users.Models;

public sealed record UserQueryViewModel(
    string Id,
    string Email,
    string Username,
    int HandsPlayed,
    int HandsWon,
    decimal TotalEarnings);