using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public class Lobby : Entity
{
    private const int MinPlayers = 2;
    private const int MaxPlayers = 6;

    private Lobby(string name, string hostingPlayerName)
    {
        Name = name;
        HostingPlayerName = hostingPlayerName;
    }

    private Lobby(List<Player> players, string hostingPlayerId, string name, string hostingPlayerName)
    {
        Players = players;
        HostingPlayerId = hostingPlayerId;
        Name = name;
        HostingPlayerName = hostingPlayerName;
    }

    public string Name { get; private set; }
    public List<Player> Players { get; private set; } =  new List<Player>();
    public string HostingPlayerId {get; private set;}
    public string HostingPlayerName {get; private set;}

    public bool IsFull => Players.Count >= MaxPlayers;
    public bool IsReadyToStart => Players.Count >= MinPlayers;

    public static Result<Lobby> CreateLobby(List<Player> players, string lobbyName, string  hostingPlayerId, string hostingPlayerName)
    {
        var lobby = new Lobby(players, hostingPlayerId, lobbyName,  hostingPlayerName);

        return Result<Lobby>.Success(lobby);
    }

    public Result AddPlayer(Player player)
    {
        if (IsFull)
            return Result.Failure(ResponseList.LobbyFull);

        if (Players.Any(p => p.Id == player.Id))
            return Result.Failure(ResponseList.PlayerAlreadyInTheLobby);

        Players.Add(player);
        return Result.Success();
    }

    public Result RemovePlayer(string playerId)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player is null)
            return Result.Failure(ResponseList.PlayerNotInLobby);

        Players.Remove(player);
        return Result.Success();
    }
}