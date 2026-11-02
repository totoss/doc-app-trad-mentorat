using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Application.Products;

public sealed record CreateProductInput(string Name);

/// <summary>Use case = port entrant : une classe par intention, appelée directement par l'adapter (endpoint, job…).</summary>
public sealed class CreateProduct(IProductRepository products, ICatalogUnitOfWork unitOfWork, IClock clock)
{
    public async Task<Result<ProductDto>> ExecuteAsync(CreateProductInput input, CancellationToken cancellationToken)
    {
        var name = ProductName.Create(input.Name);
        if (name.IsFailure)
        {
            return name.Error!;
        }

        var product = Product.Create(name.Value, clock.UtcNow);
        products.Add(product);
        await unitOfWork.CommitAsync(cancellationToken);

        return ProductDto.From(product);
    }
}
