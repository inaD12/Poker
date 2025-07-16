using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;

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