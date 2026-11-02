using HexaApp.Catalog.Application.Products;
using HexaApp.Catalog.Application.Tests.Fakes;
using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Application.Tests;

public class ProductUseCaseTests
{
    private readonly InMemoryProductRepository _products = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly FixedClock _clock = new(new DateTimeOffset(2026, 11, 2, 9, 0, 0, TimeSpan.Zero));

    [Fact]
    public async Task CreateProduct_stores_a_draft_and_commits()
    {
        var result = await new CreateProduct(_products, _unitOfWork, _clock)
            .ExecuteAsync(new CreateProductInput("Café"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Draft", result.Value.Status);
        Assert.Single(_products.All);
        Assert.Equal(1, _unitOfWork.Commits);
    }

    [Fact]
    public async Task CreateProduct_with_an_invalid_name_commits_nothing()
    {
        var result = await new CreateProduct(_products, _unitOfWork, _clock)
            .ExecuteAsync(new CreateProductInput(" "), CancellationToken.None);

        Assert.Equal(ProductErrors.NameRequired, result.Error);
        Assert.Empty(_products.All);
        Assert.Equal(0, _unitOfWork.Commits);
    }

    [Fact]
    public async Task PublishProduct_on_an_unknown_id_is_not_found()
    {
        var result = await new PublishProduct(_products, _unitOfWork, _clock)
            .ExecuteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Equal(ErrorType.NotFound, result.Error?.Type);
        Assert.Equal(0, _unitOfWork.Commits);
    }

    [Fact]
    public async Task PublishProduct_publishes_at_the_clock_time_and_commits()
    {
        var product = Product.Create(ProductName.Create("Café").Value, _clock.UtcNow);
        _products.Add(product);
        _clock.UtcNow = _clock.UtcNow.AddDays(1);

        var result = await new PublishProduct(_products, _unitOfWork, _clock)
            .ExecuteAsync(product.Id.Value, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(_clock.UtcNow, product.PublishedAt);
        Assert.Equal(1, _unitOfWork.Commits);
    }
}
