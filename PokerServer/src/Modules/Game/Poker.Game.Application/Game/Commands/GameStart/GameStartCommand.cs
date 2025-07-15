using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Game.Application.Game.Models;

namespace Poker.Game.Application.Game.Commands.GameStart;

public sealed record GameStartCommand(
    List<string> PlayerIds) : ICommand<GameCommandViewModel>;
