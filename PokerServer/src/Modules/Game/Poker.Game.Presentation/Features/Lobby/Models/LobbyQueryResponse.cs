using Poker.Common.Domain.Dtos;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Presentation.Features.Lobby.Models;

public record LobbyQueryResponse(
    string Id,
    DateTime CreatedAt,
    List<PlayerInfoDto> Players,
    bool IsFull,
    bool IsReadyToStart);
