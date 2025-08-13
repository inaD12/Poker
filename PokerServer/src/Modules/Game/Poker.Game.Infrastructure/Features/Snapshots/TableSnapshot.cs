using Newtonsoft.Json;
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
        TableJson = JsonConvert.SerializeObject(table);
        Id = table.Id;
    }

    public string TableJson { get; } = null!;

    public Table ToDomain()
    {
        var table = JsonConvert.DeserializeObject<Table>(TableJson)!;
        
        return table;
    }
}