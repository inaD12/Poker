using MediatR;
using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Events;
using Serilog;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class NewHandDomainEventHandler : INotificationHandler<NewHandDomainEvent>
{
    private readonly ITableNotifier _notifier;
    private readonly IEntityStore<Table> _tableStore;

    public NewHandDomainEventHandler(ITableNotifier notifier, IEntityStore<Table> tableStore)
    {
        _notifier = notifier;
        _tableStore = tableStore;
    }

    public async Task Handle(NewHandDomainEvent notification, CancellationToken cancellationToken)
    {
        var table = await _tableStore.GetAsync(notification.TableId, cancellationToken);
        if (table is null)
        {
            Log.Error("NewHandDomainEventHandler error: Table not found for TableId: {TableId}", notification.TableId);
            throw new InvalidOperationException("Internal error.");

        }

        foreach (var player in table.Players)
        {
            var resGameStateDto = table.GetGameState(player.Id);
            if (resGameStateDto.IsFailure)
            {
                Log.Error("NewHandDomainEventHandler error: {error}", resGameStateDto.Response);
                throw new InvalidOperationException("Internal error.");
            }
            
            await _notifier.NotifyGameStartedAsync(player.Id, resGameStateDto.Value!);
        }
    }
}