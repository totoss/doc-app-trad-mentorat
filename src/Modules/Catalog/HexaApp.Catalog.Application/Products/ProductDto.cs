using HexaApp.Catalog.Domain.Products;

namespace HexaApp.Catalog.Application.Products;

public sealed record ProductDto(Guid Id, string Name, string Status, DateTimeOffset CreatedAt, DateTimeOffset? PublishedAt)
{
    public static ProductDto From(Product product) => new(
        product.Id.Value,
        product.Name.Value,
        product.Status.ToString(),
        product.CreatedAt,
        product.PublishedAt);
}
