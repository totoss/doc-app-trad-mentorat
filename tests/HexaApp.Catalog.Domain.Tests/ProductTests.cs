using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Domain.Tests;

public class ProductNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_rejects_empty_names(string? value)
    {
        var result = ProductName.Create(value);

        Assert.Equal(ProductErrors.NameRequired, result.Error);
    }

    [Fact]
    public void Create_rejects_names_longer_than_the_limit()
    {
        var result = ProductName.Create(new string('a', ProductName.MaxLength + 1));

        Assert.Equal(ProductErrors.NameTooLong, result.Error);
    }

    [Fact]
    public void Create_trims_the_name()
    {
        Assert.Equal("Café", ProductName.Create("  Café ").Value.Value);
    }
}

public class ProductTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 2, 9, 0, 0, TimeSpan.Zero);

    private static Product NewProduct() => Product.Create(ProductName.Create("Café").Value, Now);

    [Fact]
    public void A_new_product_is_a_draft_without_events()
    {
        var product = NewProduct();

        Assert.Equal(ProductStatus.Draft, product.Status);
        Assert.Equal(Now, product.CreatedAt);
        Assert.Empty(product.DomainEvents);
    }

    [Fact]
    public void Publish_marks_the_product_published_and_raises_an_event()
    {
        var product = NewProduct();
        var later = Now.AddHours(1);

        var result = product.Publish(later);

        Assert.True(result.IsSuccess);
        Assert.Equal(ProductStatus.Published, product.Status);
        Assert.Equal(later, product.PublishedAt);
        var published = Assert.IsType<ProductPublished>(Assert.Single(product.DomainEvents));
        Assert.Equal(product.Id, published.ProductId);
    }

    [Fact]
    public void Publish_twice_is_a_conflict_and_raises_nothing_more()
    {
        var product = NewProduct();
        product.Publish(Now);

        var result = product.Publish(Now.AddHours(1));

        Assert.Equal(ErrorType.Conflict, result.Error?.Type);
        Assert.Equal(Now, product.PublishedAt);
        Assert.Single(product.DomainEvents);
    }
}
