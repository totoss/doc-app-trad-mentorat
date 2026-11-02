using System.Net;
using System.Net.Http.Json;
using HexaApp.Catalog.Application;
using HexaApp.Catalog.Application.Products;
using HexaApp.Catalog.Application.Tests.Fakes;
using HexaApp.Catalog.Domain.Products;
using HexaApp.SharedKernel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HexaApp.Api.IntegrationTests;

/// <summary>
/// Teste l'adapter HTTP et le câblage de la racine de composition, avec la persistance remplacée
/// par ses fakes : c'est l'intérêt des ports. Le vrai adapter PostgreSQL se teste à part (Testcontainers).
/// </summary>
public sealed class CatalogApiFactory : WebApplicationFactory<Program>
{
    public InMemoryProductRepository Products { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Hors Development : pas de migration au démarrage. La chaîne n'est jamais ouverte.
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:Database", "Host=unused");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IProductRepository>();
            services.RemoveAll<ICatalogUnitOfWork>();
            services.AddSingleton<IProductRepository>(Products);
            services.AddSingleton<ICatalogUnitOfWork, FakeUnitOfWork>();
        });
    }
}

public class CatalogEndpointsTests(CatalogApiFactory factory) : IClassFixture<CatalogApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Create_get_then_publish_a_product()
    {
        var created = await _client.PostAsJsonAsync("/catalog/products", new { name = "Café" });
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        var product = await created.Content.ReadFromJsonAsync<ProductDto>();
        Assert.Equal($"/catalog/products/{product!.Id}", created.Headers.Location?.OriginalString);

        var fetched = await _client.GetFromJsonAsync<ProductDto>($"/catalog/products/{product.Id}");
        Assert.Equal("Draft", fetched!.Status);

        var published = await _client.PostAsync(new Uri($"/catalog/products/{product.Id}/publish", UriKind.Relative), null);
        Assert.Equal(HttpStatusCode.NoContent, published.StatusCode);

        var again = await _client.PostAsync(new Uri($"/catalog/products/{product.Id}/publish", UriKind.Relative), null);
        Assert.Equal(HttpStatusCode.Conflict, again.StatusCode);
    }

    [Fact]
    public async Task Business_errors_become_problem_details()
    {
        var invalid = await _client.PostAsJsonAsync("/catalog/products", new { name = "" });
        Assert.Equal(HttpStatusCode.BadRequest, invalid.StatusCode);
        Assert.Equal("application/problem+json", invalid.Content.Headers.ContentType?.MediaType);
        Assert.Contains("product.name_required", await invalid.Content.ReadAsStringAsync(), StringComparison.Ordinal);

        var missing = await _client.GetAsync(new Uri($"/catalog/products/{Guid.NewGuid()}", UriKind.Relative));
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task The_dispatcher_delivers_events_to_every_registered_handler()
    {
        var handler = new RecordingHandler();
        using var app = factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton<IDomainEventHandler<ProductPublished>>(handler)));
        await using var scope = app.Services.CreateAsyncScope();
        var domainEvent = new ProductPublished(ProductId.New(), DateTimeOffset.UtcNow);

        await scope.ServiceProvider.GetRequiredService<IDomainEventDispatcher>()
            .DispatchAsync([domainEvent], CancellationToken.None);

        Assert.Equal(domainEvent, Assert.Single(handler.Received));
    }

    private sealed class RecordingHandler : IDomainEventHandler<ProductPublished>
    {
        public List<ProductPublished> Received { get; } = [];

        public Task HandleAsync(ProductPublished domainEvent, CancellationToken cancellationToken)
        {
            Received.Add(domainEvent);
            return Task.CompletedTask;
        }
    }
}
