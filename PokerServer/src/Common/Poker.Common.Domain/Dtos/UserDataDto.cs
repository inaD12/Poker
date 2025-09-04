namespace Poker.Common.Domain.Dtos;

public record UserDataDto(
    string Id,
    string UserName,
    int HandsPlayed,
    int HandsWon,
    decimal TotalEarnings);