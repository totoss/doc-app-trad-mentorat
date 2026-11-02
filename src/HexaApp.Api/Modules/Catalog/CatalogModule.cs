using HexaApp.Api.Common;
using HexaApp.Catalog.Application.Products;
using HexaApp.Catalog.Infrastructure;
using HexaApp.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HexaApp.Api.Modules.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Chaîne de connexion 'Database' manquante.");

        services.AddCatalogInfrastructure(connectionString);

        services.AddScoped<CreateProduct>();
        services.AddScoped<PublishProduct>();
        services.AddScoped<GetProduct>();
        return services;
    }

    /// <summary>Adapter entrant HTTP : traduit la requête en input, appelle le use case, traduit le Result en réponse.</summary>
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("/catalog/products").WithTags("Catalog");

        products.MapPost("/", async (CreateProductInput input, CreateProduct useCase, CancellationToken ct) =>
        {
            var result = await useCase.ExecuteAsync(input, ct);
            return result.IsSuccess
                ? Results.Created($"/catalog/products/{result.Value.Id}", result.Value)
                : result.ToProblem();
        });

        products.MapGet("/{id:guid}", async (Guid id, GetProduct useCase, CancellationToken ct) =>
            (await useCase.ExecuteAsync(id, ct)).ToHttpResult());

        products.MapPost("/{id:guid}/publish", async (Guid id, PublishProduct useCase, CancellationToken ct) =>
            (await useCase.ExecuteAsync(id, ct)).ToHttpResult());

        return app;
    }

    /// <summary>Confort de développement uniquement : en production, appliquer les migrations dans le pipeline de déploiement.</summary>
    public static async Task MigrateCatalogDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<CatalogDbContext>().Database.MigrateAsync();
    }
}
