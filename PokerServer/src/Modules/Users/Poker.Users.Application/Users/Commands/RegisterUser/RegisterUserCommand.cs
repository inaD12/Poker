using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Users.Application.Users.Models;

namespace Poker.Users.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string Username) : ICommand<UserCommandViewModel>;