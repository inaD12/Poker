using Poker.Common.Domain.Dtos;

namespace Poker.Game.Application.Features.Lobby.Models;

public record LobbyViewModel(
        string Id,
        string Name,
        string Creator,
        DateTime CreatedAt,
        List<PlayerInfoDto> Players,
        bool IsFull,
        bool IsReadyToStart);
