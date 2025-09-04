using Poker.Common.Domain.Abstractions.Messaging;

namespace Poker.Users.Application.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(
    string Id) : ICommand;