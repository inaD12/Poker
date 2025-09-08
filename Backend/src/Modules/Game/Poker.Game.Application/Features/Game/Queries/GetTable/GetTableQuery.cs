using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Dtos;

namespace Poker.Game.Application.Features.Game.Queries.GetTable;

public sealed record GetTableQuery(
    string TableId,
    string PlayerId) : IQuery<GameStateDto>;