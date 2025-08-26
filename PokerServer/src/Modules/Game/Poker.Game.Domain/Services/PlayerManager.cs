using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Services;

public class PlayerManager
{
    public PlayerManager(List<Player> players, int currentTurnPlayerPosition, string hostPlayerId, int dealerPosition, HashSet<string> playersWhoActed)
    {
        _players = players;
        _playerDictionary = players.ToDictionary(p => p.Id);
        CurrentTurnPlayerPosition = currentTurnPlayerPosition;
        HostPlayerId = hostPlayerId;
        DealerPosition = dealerPosition;
        PlayersWhoActed = playersWhoActed;
    }

    private readonly List<Player> _players;
    private readonly Dictionary<string, Player> _playerDictionary;

    public HashSet<string> PlayersWhoActed {get; private set;}
    public int CurrentTurnPlayerPosition {get; private set;}
    public string HostPlayerId {get; private set;}
    public int DealerPosition {get; set;}

    internal IReadOnlyCollection<Player> Players => _players.AsReadOnly();
    internal Player Dealer => _players[DealerPosition];
    internal Player CurrentTurnPlayer =>  _players[CurrentTurnPlayerPosition];
    internal Player? GetPlayer(string playerId) =>
        _playerDictionary.GetValueOrDefault(playerId);
    internal int ActivePlayerCount => 
        Players.Count(p => p.Hand is not null && !p.Hand.IsFolded && !p.Hand.IsAllIn && !p.IsDisconnected);

    
    internal void MarkPlayerActed(string playerId) => PlayersWhoActed.Add(playerId);
    
    internal void ResetPlayersActed() => PlayersWhoActed.Clear();

    internal void SetNextActivePosition()
    {
        for (var i = 1; i <= _players.Count; i++)
        {
            var next = (CurrentTurnPlayerPosition + i) % _players.Count;
            var p = _players[next];
            
            if (p.IsDisconnected)
                p.Hand!.Fold();

            if (!p.Hand!.IsFolded && !p.Hand.IsAllIn && !p.IsDisconnected)
            {
                CurrentTurnPlayerPosition = next;
                return;
            }
        }

        throw new InvalidOperationException("No eligible players.");
    }

    internal bool IsBettingRoundComplete(int currentBet)
    {
        var activePlayers = _players
            .Where(p => !p.Hand!.IsFolded && !p.Hand.IsAllIn)
            .ToList();

        return activePlayers.All(p => PlayersWhoActed.Contains(p.Id) && p.Hand!.Bet == currentBet);
    }

    internal bool IsPlayerTurn(string playerId)
    {
        if (_players[CurrentTurnPlayerPosition].Id != playerId)
            return false;

        return true;
    }

    internal void ResetHandsForNextRound()
    {
        foreach (var player in _players)
            if (player.Hand != null)
                player.Hand.ResetBet();
    }

    internal void SetFirstActivePlayer()
    {
        for (var i = 0; i < _players.Count; i++)
        {
            var p = _players[i];
            if (p.Hand != null && !p.Hand.IsFolded && !p.Hand.IsAllIn && !p.IsDisconnected)
            {
                CurrentTurnPlayerPosition = i;
                return;
            }
        }
    }

    internal Result<Player> GetPlayerIfHisTurn(string playerId)
    {
        if (!_playerDictionary.TryGetValue(playerId, out var player))
            return Result<Player>.Failure(ResponseList.PlayerNotInGame);
        if (!IsPlayerTurn(playerId))
            return Result<Player>.Failure(ResponseList.NotYourTurn);

        return Result<Player>.Success(player);
    }
    
    internal Result KickPlayer(string playerId)
    {
        var result = RemovePlayer(playerId);
        if (result.IsFailure)
            return result;

        if (DealerPosition >= _players.Count)
            DealerPosition = 0;

        if (HostPlayerId == playerId && _players.Any())
            HostPlayerId = _players[0].Id;

        return Result.Success();
    }
    
    private Result RemovePlayer(string playerId)
    {
        var index = _players.FindIndex(p => p.Id == playerId);
        if (index == -1)
            return Result.Failure(ResponseList.PlayerNotInGame);

        _playerDictionary.Remove(playerId);
        _players.RemoveAt(index);

        if (index == CurrentTurnPlayerPosition)
        {
            CurrentTurnPlayerPosition %= _players.Count;
            SetNextActivePosition();
        }
        else if (index < CurrentTurnPlayerPosition)
            CurrentTurnPlayerPosition--;

        return Result.Success();
    }
    
    internal Result PlayerDisconnected(string playerId)
    {
        if (!_playerDictionary.TryGetValue(playerId, out var player))
            return Result.Failure(ResponseList.PlayerNotInGame);

        player.Disconnect();

        if (IsPlayerTurn(playerId))
            SetNextActivePosition();
        
        return Result.Success();
    }

    internal Result PlayerReconnected(string playerId)
    {
        if (!_playerDictionary.TryGetValue(playerId, out var player))
            return Result.Failure(ResponseList.PlayerNotInGame);

        player.Reconnect();
        
        return Result.Success();
    }
}