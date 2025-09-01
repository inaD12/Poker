namespace Poker.Common.Domain.Dtos;

public class PlayerInfoDto(
    string id,
    string username,
    int balance,
    int handsPlayed,
    int handsWon,
    decimal totalEarnings,
    bool isSelf = false)
{
    public string Id { get; init; } = id;
    public string Username { get; init; } = username;
    public int Balance { get; init; } = balance;
    public int HandsPlayed { get; init; } = handsPlayed;
    public int HandsWon { get; init; } = handsWon;
    public decimal TotalEarnings { get; init; } = totalEarnings;
    public bool IsSelf { get; set; } = isSelf;
}