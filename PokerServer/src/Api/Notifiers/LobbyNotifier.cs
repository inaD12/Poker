using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Dtos;
using PokerServer.Hubs;

namespace PokerServer.Notifiers;

public class LobbyNotifier: ILobbyNotifier
{
    private readonly IHubContext<LobbyHub, ILobbyClient> _hubContext;

    public LobbyNotifier(IHubContext<LobbyHub, ILobbyClient> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task NotifyPlayerJoinedAsync(string lobbyId, PlayerInfoDto player)
    {
        await _hubContext.Clients.Group(lobbyId).PlayerJoined(player);
    }

    public async Task NotifyPlayerLeftAsync(string lobbyId, string playerId)
    {
        await _hubContext.Clients.Group(lobbyId).PlayerLeft(playerId);
    }

    public async Task NotifyLobbyClosedAsync(string lobbyId)
    {
        await _hubContext.Clients.Group(lobbyId).LobbyClosed();
    }

    public async Task NotifyGameStartingAsync(string lobbyId, string gameId)
    {
        await _hubContext.Clients.Group(lobbyId).GameStarted(gameId);
    }
}