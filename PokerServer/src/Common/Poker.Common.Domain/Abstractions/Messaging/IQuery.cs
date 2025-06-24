using MediatR;
using Poker.Common.Domain.Results;

namespace Poker.Common.Domain.Abstractions.Messaging;

public interface IQuery<Tresponse> : IRequest<Result<Tresponse>>
{
}
