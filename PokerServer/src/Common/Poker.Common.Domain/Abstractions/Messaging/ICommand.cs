using MediatR;
using Poker.Common.Domain.Results;

namespace Poker.Common.Domain.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}