using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Lobby.Models;

namespace Poker.Game.Application.Lobby.Commands.CreateLobby;

public sealed record CreateLobbyCommand(
    string StartingPlayerId) : ICommand<LobbyCommandViewModel>;
