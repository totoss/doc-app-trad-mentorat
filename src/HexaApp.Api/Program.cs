using HexaApp.Api.Common;
using HexaApp.Api.Modules.Catalog;
using HexaApp.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// Adapters transverses.
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddScoped<IDomainEventDispatcher, ServiceProviderDomainEventDispatcher>();

// Un appel par module : chacun branche ses use cases et ses adapters.
builder.Services.AddCatalogModule(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.MigrateCatalogDatabaseAsync();
}

app.MapCatalogEndpoints();

await app.RunAsync();
