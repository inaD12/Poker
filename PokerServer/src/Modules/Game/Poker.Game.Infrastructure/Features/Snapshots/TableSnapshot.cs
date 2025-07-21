using System.Text.Json;
using Poker.Common.Domain;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Infrastructure.Features.Snapshots;

public class TableSnapshot : Entity
{
    public string TableJson { get; private set; } = null!;

    private TableSnapshot() { }

    public TableSnapshot(Table table)
    {
        TableJson = JsonSerializer.Serialize(table);
    }

    public Table ToDomain()
        => JsonSerializer.Deserialize<Table>(TableJson)!;
}
