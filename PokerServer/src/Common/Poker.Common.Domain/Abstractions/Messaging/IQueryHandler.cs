using MediatR;
using Poker.Common.Domain.Results;

namespace Poker.Common.Domain.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}