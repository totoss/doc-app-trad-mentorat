using HexaApp.SharedKernel;

namespace HexaApp.Api.Common;

/// <summary>Publication en mémoire, dans la transaction : chaque événement va à tous les handlers enregistrés pour son type.</summary>
public sealed class ServiceProviderDomainEventDispatcher(IServiceProvider services) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handle = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))!;

            foreach (var handler in services.GetServices(handlerType))
            {
                await (Task)handle.Invoke(handler, [domainEvent, cancellationToken])!;
            }
        }
    }
}
