using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Application.Features.Lobby.Commands.AddFundsToPlayer;

public sealed record AddFundsToPlayerCommand(
    string PlayerId,
    string LobbyId,
    int Funds) : ICommand;