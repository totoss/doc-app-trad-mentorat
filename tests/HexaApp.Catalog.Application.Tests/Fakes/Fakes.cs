using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Application.Tests.Fakes;

// Adapters en mémoire des ports sortants : les use cases se testent sans base ni réseau.
// Aussi réutilisés par HexaApp.Api.IntegrationTests pour remplacer PostgreSQL.

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<ProductId, Product> _products = [];

    public IReadOnlyCollection<Product> All => _products.Values;

    public Task<Product?> GetAsync(ProductId id, CancellationToken cancellationToken) =>
        Task.FromResult(_products.GetValueOrDefault(id));

    public void Add(Product product) => _products.Add(product.Id, product);
}

public sealed class FakeUnitOfWork : ICatalogUnitOfWork
{
    public int Commits { get; private set; }

    public Task CommitAsync(CancellationToken cancellationToken)
    {
        Commits++;
        return Task.CompletedTask;
    }
}

public sealed class FixedClock(DateTimeOffset now) : IClock
{
    public DateTimeOffset UtcNow { get; set; } = now;
}
