namespace HexaApp.SharedKernel;

/// <summary>Vue non générique d'un agrégat, pour collecter ses événements sans connaître son type d'identifiant.</summary>
public interface IAggregateRoot
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

/// <summary>Racine d'agrégat : frontière de cohérence, identifiée par <typeparamref name="TId"/>, qui émet des événements de domaine.</summary>
public abstract class AggregateRoot<TId> : IAggregateRoot
    where TId : struct
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot(TId id) => Id = id;

    public TId Id { get; private init; }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
