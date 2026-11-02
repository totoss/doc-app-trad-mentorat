using HexaApp.Catalog.Application;
using HexaApp.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HexaApp.Catalog.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Branche les adapters sortants du module sur ses ports.</summary>
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(
            connectionString,
            npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", CatalogDbContext.Schema)));

        services.AddScoped<ICatalogUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>());
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}
