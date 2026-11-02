using HexaApp.Catalog.Domain.Products;

namespace HexaApp.Catalog.Application;

// Ports sortants : déclarés ici, implémentés par les adapters de HexaApp.Catalog.Infrastructure.

public interface IProductRepository
{
    Task<Product?> GetAsync(ProductId id, CancellationToken cancellationToken);
    void Add(Product product);
}

/// <summary>Une unité de travail par module : un use case = un commit, événements de domaine publiés avant.</summary>
public interface ICatalogUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
