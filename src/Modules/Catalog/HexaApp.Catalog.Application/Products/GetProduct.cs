using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Application.Products;

public sealed class GetProduct(IProductRepository products)
{
    public async Task<Result<ProductDto>> ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var id = new ProductId(productId);
        var product = await products.GetAsync(id, cancellationToken);
        return product is null ? ProductErrors.NotFound(id) : ProductDto.From(product);
    }
}
