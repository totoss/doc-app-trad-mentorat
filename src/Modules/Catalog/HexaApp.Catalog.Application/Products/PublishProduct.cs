using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Application.Products;

public sealed class PublishProduct(IProductRepository products, ICatalogUnitOfWork unitOfWork, IClock clock)
{
    public async Task<Result> ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var id = new ProductId(productId);
        var product = await products.GetAsync(id, cancellationToken);
        if (product is null)
        {
            return ProductErrors.NotFound(id);
        }

        var published = product.Publish(clock.UtcNow);
        if (published.IsFailure)
        {
            return published;
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
