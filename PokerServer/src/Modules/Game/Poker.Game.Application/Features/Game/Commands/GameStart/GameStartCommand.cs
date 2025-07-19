using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Features.Game.Models;

namespace Poker.Game.Application.Features.Game.Commands.GameStart;

public sealed record GameStartCommand(
    string LobbyId) : ICommand<GameCommandViewModel>;
