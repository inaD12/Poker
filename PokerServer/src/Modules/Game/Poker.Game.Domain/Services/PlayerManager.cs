using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Services;

public class PlayerManager
{
	private int _currentTurnPlayerPosition;
	private readonly Dictionary<string, Player> _playerDictionary;
	private readonly List<Player> _players;

	public PlayerManager(List<Player> players, int currentTurnPlayerPosition)
	{
		_players = players;
		_playerDictionary = players.ToDictionary(p => p.Id);
		_currentTurnPlayerPosition = currentTurnPlayerPosition;
	}

	public IReadOnlyList<Player> GetPlayers()
		=> _players.AsReadOnly();

	public Player GetCurrentTurnPlayer()
		=> _players[_currentTurnPlayerPosition];

	public int GetNextActivePosition()
	{
		for (int i = 1; i <= _players.Count; i++)
		{
			var next = (_currentTurnPlayerPosition + i) % _players.Count;
			var p = _players[next];
			if (!p.Hand!.IsFolded && !p.Hand.IsAllIn)
				return next;
		}

		throw new InvalidOperationException("No eligible players.");
	}

	public bool IsBettingRoundComplete(int currentBet)
	{
		var activePlayers = _players
			.Where(p => !p.Hand!.IsFolded && !p.Hand.IsAllIn)
			.ToList();

		return activePlayers.All(p => p.Hand!.Bet == currentBet);
	}

	public bool OnlyOneActivePlayer()
	{
		return _players.Count(p => !p.Hand!.IsFolded) == 1;
	}

	public bool IsPlayerTurn(string playerId)
	{
		if (_players[_currentTurnPlayerPosition].Id != playerId)
			return false;

		return true;
	}

	public void ResetHandsForNextRound()
	{
		foreach (var player in _players)
		{
			if (player.Hand != null)
				player.Hand.ResetBet();
		}
	}

	public void SetFirstActivePlayer()
	{
		for (int i = 0; i < _players.Count; i++)
		{
			var player = _players[i];
			if (player.Hand == null || player.Hand.IsFolded || player.Hand.IsAllIn)
				continue;

			_currentTurnPlayerPosition = i;
			return;
		}
	}
	public Result<Player> GetPlayerIfHisTurn(string playerId)
	{
		if (!_playerDictionary.TryGetValue(playerId, out var player))
			return Result<Player>.Failure(ResponseList.PlayerNotInGame);
		if (!IsPlayerTurn(playerId))
			return Result<Player>.Failure(ResponseList.NotYourTurn);

		return Result<Player>.Success(player);
	}
}
