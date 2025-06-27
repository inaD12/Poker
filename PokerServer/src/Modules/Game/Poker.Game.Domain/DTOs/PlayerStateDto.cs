using Poker.Game.Domain.Entities;

namespace Poker.Game.Domain.DTOs;

public record PlayerStateDto(
	string Id,
	int Balance,
	bool IsFolded,
	bool IsAllIn,
	int CurrentBet,
	bool IsCurrentTurn,
	IReadOnlyList<Card>? Cards
);