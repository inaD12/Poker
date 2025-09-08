using Poker.Common.Domain.Dtos;

namespace Poker.Game.Presentation.Features.Lobby.Models;

public record LobbyQueryResponse(
    string Id,
    string Name,
    string Creator,
    DateTime CreatedAt,
    List<PlayerInfoDto> Players,
    bool IsFull,
    bool IsReadyToStart);
