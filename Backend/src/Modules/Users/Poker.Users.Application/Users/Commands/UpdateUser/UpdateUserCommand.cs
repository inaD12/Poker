using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Users.Application.Users.Models;

namespace Poker.Users.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    string Id,
    string NewUsername) : ICommand<UserCommandViewModel>;