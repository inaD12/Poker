namespace Poker.Common.Domain.Dtos;

public record PlayerInfoDto(
    string Id,
    string Username,
    int GamesPlayed,
    int GamesWon,
    decimal TotalEarnings);