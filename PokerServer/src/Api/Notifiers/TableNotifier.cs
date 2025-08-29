using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Notifications;
using PokerServer.Hubs;

namespace PokerServer.Notifiers;

public class TableNotifier : ITableNotifier
{
    private readonly IHubContext<GameHub, IGameClient> _hubContext;

    public TableNotifier(IHubContext<GameHub, IGameClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyGameStartedAsync(string playerId, GameStateDto state)
    {
        await _hubContext.Clients.User(playerId).ReceiveGameState(state);
    }

    public async Task NotifyPlayerActionAsync(string tableId, string playerId, PlayerActionNotification action)
    {
        await _hubContext.Clients.Group(tableId).PlayerAction(playerId, action);
    }

    public async Task NotifyGamePhaseUpdateAsync(string tableId, GamePhase gamePhase, List<CardDto> cards)
    {
        await _hubContext.Clients.Group(tableId).GamePhaseUpdate(gamePhase, cards);
    }

    public async Task NotifyShowdownAsync(string tableId, List<string> winnerPlayerIds, int winningsEach)
    {
        await _hubContext.Clients.Group(tableId).Showdown(winnerPlayerIds, winningsEach);
    }

    public async Task NotifyNextPlayerAsync(string playerId)
    {
        await _hubContext.Clients.User(playerId).YourTurn();
    }

    public async Task NotifyGameClosingAsync(string tableId)
    {
        await _hubContext.Clients.Group(tableId).GameClose();
    }
}