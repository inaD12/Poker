using Poker.Common.Domain;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Entities;

public class Lobby : Entity
{
    private Lobby() { }
    private Lobby(List<Player> players)
    {
        Players = players;
    }

    public static Result<Lobby> CreateLobby(List<Player> players)
    {
        var lobby =  new Lobby(players);
        
        return  Result<Lobby>.Success(lobby);
    }
    
    private const int MinPlayers = 2;
    private const int MaxPlayers = 6;

    public List<Player> Players { get; private set; }

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

    public Result RemovePlayer(string playerId)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player is  null) 
            return Result.Failure(ResponseList.PlayerNotInLobby);
            
        Players.Remove(player);
        return Result.Success();
    }
}
