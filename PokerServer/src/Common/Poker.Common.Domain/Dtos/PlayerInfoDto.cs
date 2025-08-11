namespace Poker.Common.Domain.Dtos;

public class PlayerInfoDto(
    string id,
    string username,
    int gamesPlayed,
    int gamesWon,
    decimal totalEarnings,
    bool isSelf = false)
{
    public string Id { get; init; } = id;
    public string Username { get; init; } = username;
    public int GamesPlayed { get; init; } = gamesPlayed;
    public int GamesWon { get; init; } = gamesWon;
    public decimal TotalEarnings { get; init; } = totalEarnings;
    public bool IsSelf { get; set; } = isSelf;
}