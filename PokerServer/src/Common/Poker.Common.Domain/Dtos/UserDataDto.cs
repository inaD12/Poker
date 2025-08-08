namespace Poker.Common.Domain.Dtos;

public record UserDataDto(
    string Id,
    string UserName,
    int GamesPlayed,
    int GamesWon,
    decimal TotalEarnings);