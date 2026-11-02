using HexaApp.Catalog.Application;
using HexaApp.Catalog.Domain.Products;

namespace HexaApp.Catalog.Infrastructure.Persistence;

internal sealed class ProductRepository(CatalogDbContext db) : IProductRepository
{
    public async Task<Product?> GetAsync(ProductId id, CancellationToken cancellationToken) =>
        await db.Products.FindAsync([id], cancellationToken);

    public void Add(Product product) => db.Products.Add(product);
}
