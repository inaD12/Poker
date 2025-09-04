using MediatR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Game.Domain.Events;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class ShowdownDomainEventHandler : INotificationHandler<ShowdownDomainEvent>
{
    private readonly ITableNotifier _notifier;

    public ShowdownDomainEventHandler(ITableNotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task Handle(ShowdownDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyShowdownAsync(
            notification.TableId,
            notification.WinnerPlayerIds,
            notification.WinningsEach,
            notification.players);
    }
}