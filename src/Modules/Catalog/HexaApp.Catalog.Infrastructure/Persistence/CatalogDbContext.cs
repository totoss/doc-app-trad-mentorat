using HexaApp.Catalog.Application;
using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace HexaApp.Catalog.Infrastructure.Persistence;

/// <summary>Un DbContext par module, dans son propre schéma PostgreSQL ; il sert aussi d'unité de travail.</summary>
public sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options, IDomainEventDispatcher dispatcher)
    : DbContext(options), ICatalogUnitOfWork
{
    public const string Schema = "catalog";

    public DbSet<Product> Products => Set<Product>();

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        var aggregates = ChangeTracker.Entries<IAggregateRoot>().Select(e => e.Entity).ToList();
        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        // shortcut: une seule passe ; les événements levés par les handlers eux-mêmes ne sont pas republiés. Boucler si un handler en émet.
        await dispatcher.DispatchAsync(domainEvents, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}
