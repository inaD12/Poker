using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Application.Features.Lobby.Commands.AddPlayerToLobby;

public sealed record AddPlayerToLobbyCommand(
    string LobbyId,
    string PlayerId) : ICommand<LobbyViewModel>;