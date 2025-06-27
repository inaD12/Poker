using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Domain.DTOs;

public record GameStateDto(
	GamePhase Phase,
	IReadOnlyList<Card> CommunityCards,
	int CurrentPot,
	int CurrentBet,
	int MinimumRaise,
	string? CurrentTurnPlayerId,
	IReadOnlyList<PlayerStateDto> Players
);
