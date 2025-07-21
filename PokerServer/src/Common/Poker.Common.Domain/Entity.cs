using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Common.Domain;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Entity()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}