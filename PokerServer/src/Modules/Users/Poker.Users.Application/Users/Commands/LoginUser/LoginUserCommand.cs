using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Users.Application.Users.Models;

namespace Poker.Users.Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password) : ICommand<LoginUserCommandViewModel>;