using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Users.Application.Users.Models;

namespace Poker.Users.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(string Id) : IQuery<UserQueryViewModel>;
