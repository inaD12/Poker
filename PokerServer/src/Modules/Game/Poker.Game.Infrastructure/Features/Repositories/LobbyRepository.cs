using Microsoft.EntityFrameworkCore;
using Poker.Game.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Entities;
using Poker.Game.Infrastructure.Features.DBContexts;

namespace Poker.Game.Infrastructure.Features.Repositories;

public class LobbyRepository : ILobbyRepository
{
    private readonly GameDbContext _context;

    public LobbyRepository(GameDbContext context) => _context = context;

    public async Task AddAsync(Lobby lobby, CancellationToken cancellationToken)
    {
        await _context.Lobbies.AddAsync(lobby, cancellationToken);
    }

    public void Update(Lobby lobby)
    {
        _context.Lobbies.Update(lobby);
    }

    public async Task<Lobby?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Lobbies
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
}