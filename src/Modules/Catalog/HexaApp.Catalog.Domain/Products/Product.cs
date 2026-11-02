using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Domain.Products;

public enum ProductStatus
{
    Draft,
    Published,
}

public sealed record ProductPublished(ProductId ProductId, DateTimeOffset OccurredAt) : IDomainEvent;

/// <summary>
/// Agrégat exemple : créé en brouillon, publié une seule fois.
/// Les règles vivent ici ; les use cases ne font qu'orchestrer.
/// </summary>
public sealed class Product : AggregateRoot<ProductId>
{
    private Product(ProductId id, ProductName name, DateTimeOffset createdAt)
        : base(id)
    {
        Name = name;
        Status = ProductStatus.Draft;
        CreatedAt = createdAt;
    }

    // Réservé à EF Core, qui matérialise l'agrégat sans passer par les règles.
#pragma warning disable CS8618
    private Product()
        : base(default)
    {
    }
#pragma warning restore CS8618

    public ProductName Name { get; private set; }
    public ProductStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }

    public static Product Create(ProductName name, DateTimeOffset now) => new(ProductId.New(), name, now);

    public Result Publish(DateTimeOffset now)
    {
        if (Status == ProductStatus.Published)
        {
            return ProductErrors.AlreadyPublished;
        }

        Status = ProductStatus.Published;
        PublishedAt = now;
        Raise(new ProductPublished(Id, now));
        return Result.Success();
    }
}
