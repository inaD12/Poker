using Microsoft.EntityFrameworkCore;
using Poker.Game.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Infrastructure.Features.DBContexts;
using Poker.Game.Infrastructure.Features.Snapshots;

namespace Poker.Game.Infrastructure.Features.Repositories;

public class TableRepository : ITableRepository
{
    private readonly GameDbContext _context;

    public TableRepository(GameDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Table table, CancellationToken cancellationToken)
    {
        var snapshot = new TableSnapshot(table);
        await _context.TableSnapshots.AddAsync(snapshot, cancellationToken);
    }

    public void Update(Table table)
    {
        var snapshot = new TableSnapshot(table);
        _context.TableSnapshots.Update(snapshot);
    }

    public async Task<Table?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var snapshot = await _context.TableSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return snapshot?.ToDomain();
    }

    public async Task DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var snapshot = await _context.TableSnapshots.FindAsync(id, cancellationToken);
        if (snapshot != null)
            _context.TableSnapshots.Remove(snapshot);
    }

    public void Delete(Table table)
    {
        var snapshot = new TableSnapshot(table);
        _context.Remove(snapshot);
    }
}