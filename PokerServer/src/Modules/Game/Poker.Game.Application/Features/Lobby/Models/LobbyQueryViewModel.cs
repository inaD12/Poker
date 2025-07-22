using Poker.Common.Domain.Dtos;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Features.Lobby.Models;

public record LobbyQueryViewModel(
        string Id,
        DateTime CreatedAt,
        List<PlayerInfoDto> Players,
        bool IsFull,
        bool IsReadyToStart);
