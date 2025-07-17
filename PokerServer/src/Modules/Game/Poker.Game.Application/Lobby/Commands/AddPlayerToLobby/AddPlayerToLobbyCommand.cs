using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Game.Application.Lobby.Commands.AddPlayerToLobby;

public sealed record AddPlayerToLobbyCommand(
    string LobbyId,
    string PlayerId) : ICommand;
