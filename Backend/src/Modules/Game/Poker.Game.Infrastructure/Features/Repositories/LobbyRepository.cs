using Microsoft.EntityFrameworkCore;
using Poker.Common.Domain.Models;
using Poker.Game.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Infrastructure.Extensions;
using Poker.Game.Infrastructure.Features.DBContexts;

namespace Poker.Game.Infrastructure.Features.Repositories;

public class LobbyRepository : ILobbyRepository
{
    private readonly GameDbContext _context;

    public LobbyRepository(GameDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Lobby lobby, CancellationToken cancellationToken)
    {
        await _context.Lobbies.AddAsync(lobby, cancellationToken);
    }

    public void Update(Lobby lobby)
    {
        var existingLobby = _context.Lobbies
            .Include(l => l.Players)
            .FirstOrDefault(l => l.Id == lobby.Id);

        if (existingLobby == null)
            return;

        _context.Entry(existingLobby).CurrentValues.SetValues(lobby);

        existingLobby.Players.SyncCollection(
            lobby.Players,
            keySelector: p => p.Id,
            updateAction: (dbPlayer, incomingPlayer) =>
            {
                _context.Entry(dbPlayer).CurrentValues.SetValues(incomingPlayer);
            },
            addAction: incomingPlayer =>
            {
                _context.Entry(incomingPlayer).Property("LobbyId").CurrentValue = lobby.Id;
            },
            onRemove: dbPlayer =>
            {
                _context.Set<Player>().Remove(dbPlayer);
            }
        );
        
        var domainEvents = lobby.GetDomainEvents();
        existingLobby.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            existingLobby.RaiseDomainEvent(domainEvent);
        }
    }

    public async Task<Lobby?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Lobbies
            .AsNoTracking()
            .Include(l => l.Players)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var lobby = await _context.Lobbies.FindAsync([id], cancellationToken);
        if (lobby != null)
            _context.Lobbies.Remove(lobby);
    }

    public void Delete(Lobby lobby)
    {
        _context.Remove(lobby);
    }
    
    public async Task<PagedList<Lobby>> GetAllLobbiesPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var lobbiesQuery = _context.Lobbies
            .Include(l => l.Players)
            .OrderByDescending(l => l.CreatedAt);
        
        var lobbies = await PagedList<Lobby>.CreateAsync(lobbiesQuery, pageNumber, pageSize, cancellationToken);
        return  lobbies;
    }
}