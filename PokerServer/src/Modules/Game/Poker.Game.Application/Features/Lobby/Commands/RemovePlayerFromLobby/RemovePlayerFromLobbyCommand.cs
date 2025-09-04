using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Features.Lobby.Commands.RemovePlayerFromLobby;

public sealed record RemovePlayerFromLobbyCommand(
    string LobbyId,
    string PlayerId) : ICommand;