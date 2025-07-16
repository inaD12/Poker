using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public class Lobby : Entity
{
    private const int MinPlayers = 2;
    private const int MaxPlayers = 6;

    public List<Player> Players { get; private set; } = new();

    public bool IsFull => Players.Count >= MaxPlayers;
    public bool IsReadyToStart => Players.Count >= MinPlayers;

    public Result AddPlayer(Player player)
    {
        if (IsFull)
            return Result.Failure(ResponseList.LobbyFull);

        if (Players.Any(p => p.Id == player.Id))
            return Result.Failure(ResponseList.PlayerAlreadyInTheLobby);

        Players.Add(player);
        return Result.Success();
    }

    public void RemovePlayer(string playerId)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player is not null)
            Players.Remove(player);
    }

    public void Clear()
    {
        Players.Clear();
    }
}
