using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Dtos;

namespace Poker.Game.Application.Features.Game.Queries.GetPlayerFromGame;

public sealed record GetPlayerFromGameQuery(
    string TableId,
    string PlayerId) : IQuery<PlayerInfoDto>;