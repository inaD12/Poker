using System.Text.Json;
using Poker.Common.Domain;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Infrastructure.Features.Snapshots;

public class TableSnapshot : Entity
{
    private TableSnapshot()
    {
    }

    public TableSnapshot(Table table)
    {
        TableJson = JsonSerializer.Serialize(table);
    }

    public string TableJson { get; } = null!;

    public Table ToDomain()
    {
        return JsonSerializer.Deserialize<Table>(TableJson)!;
    }
}