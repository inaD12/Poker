using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Users.Application.Users.Models;

namespace Poker.Users.Application.Users.Commands.UserPlayedGame;

public sealed record UserPlayedHandCommand(
    string Id,
    bool Won = false,
    decimal Earnings = 0) : ICommand<UserCommandViewModel>;