using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Domain.Services;

public class PlayerManager
{
	private int CurrentTurnPlayerPosition;
	private Dictionary<string, Player> PlayerDictionary;
	private readonly List<Player> Players;

	public PlayerManager(List<Player> players, int currentTurnPlayerPosition)
	{
		Players = players;
		PlayerDictionary = players.ToDictionary(p => p.Id);
		CurrentTurnPlayerPosition = currentTurnPlayerPosition;
	}

	public IReadOnlyList<Player> GetPlayers()
		=> Players.AsReadOnly();

	public Player GetCurrentTurnPlayer()
		=> Players[CurrentTurnPlayerPosition];


	public int GetNextActivePosition()
	{
		for (int i = 1; i <= Players.Count; i++)
		{
			var next = (CurrentTurnPlayerPosition + i) % Players.Count;
			var p = Players[next];
			if (!p.Hand!.IsFolded && !p.Hand.IsAllIn)
				return next;
		}

		throw new InvalidOperationException("No eligible players.");
	}

	public bool IsBettingRoundComplete(int currentBet)
	{
		var activePlayers = Players
			.Where(p => !p.Hand!.IsFolded && !p.Hand.IsAllIn)
			.ToList();

		return activePlayers.All(p => p.Hand!.Bet == currentBet);
	}

	public bool OnlyOneActivePlayer()
	{
		return Players.Count(p => !p.Hand!.IsFolded) == 1;
	}

	public bool IsPlayerTurn(string playerId)
	{
		if (Players[CurrentTurnPlayerPosition].Id != playerId)
			return false;

		return true;
	}

	public void ResetHandsForNextRound()
	{
		foreach (var player in Players)
		{
			if (player.Hand != null)
				player.Hand.ResetBet();
		}
	}

	public void SetFirstActivePlayer()
	{
		for (int i = 0; i < Players.Count; i++)
		{
			var player = Players[i];
			if (player.Hand == null || player.Hand.IsFolded || player.Hand.IsAllIn)
				continue;

			CurrentTurnPlayerPosition = i;
			return;
		}
	}
	public Result<Player> GetPlayerIfHisTurn(string playerId)
	{
		if (!PlayerDictionary.TryGetValue(playerId, out var player))
			return Result<Player>.Failure(ResponseList.PlayerNotInGame);
		if (!IsPlayerTurn(playerId))
			return Result<Player>.Failure(ResponseList.NotYourTurn);

		return Result<Player>.Success(player);
	}
}
