using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Application.Features.Lobby.Commands.CreateLobby;

public sealed record CreateLobbyCommand(
    string StartingPlayerId) : ICommand<LobbyCommandViewModel>;