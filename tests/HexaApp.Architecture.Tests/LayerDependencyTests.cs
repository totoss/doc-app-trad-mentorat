using System.Reflection;
using HexaApp.Api.Common;
using HexaApp.Catalog.Application;
using HexaApp.Catalog.Domain.Products;
using HexaApp.Catalog.Infrastructure;
using HexaApp.SharedKernel;
using NetArchTest.Rules;

namespace HexaApp.Architecture.Tests;

/// <summary>
/// Verrouille la règle de l'hexagone : les dépendances pointent vers le domaine, jamais l'inverse.
/// Ajouter ici les assemblies de chaque nouveau module.
/// </summary>
public class LayerDependencyTests
{
    private static readonly Assembly SharedKernel = typeof(Result).Assembly;
    private static readonly Assembly Domain = typeof(Product).Assembly;
    private static readonly Assembly Application = typeof(IProductRepository).Assembly;
    private static readonly Assembly Infrastructure = typeof(DependencyInjection).Assembly;
    private static readonly Assembly Api = typeof(SystemClock).Assembly;

    private static readonly string[] Frameworks =
    [
        "Microsoft.EntityFrameworkCore",
        "Microsoft.AspNetCore",
        "Microsoft.Extensions.DependencyInjection",
        "Npgsql",
    ];

    [Fact]
    public void SharedKernel_depends_on_nothing_of_the_solution_nor_on_frameworks() =>
        AssertNoDependency(SharedKernel, [Domain, Application, Infrastructure, Api], Frameworks);

    [Fact]
    public void Domain_depends_only_on_the_shared_kernel() =>
        AssertNoDependency(Domain, [Application, Infrastructure, Api], Frameworks);

    [Fact]
    public void Application_ignores_adapters_and_frameworks() =>
        AssertNoDependency(Application, [Infrastructure, Api], Frameworks);

    [Fact]
    public void Infrastructure_does_not_know_the_api() =>
        AssertNoDependency(Infrastructure, [Api], ["Microsoft.AspNetCore"]);

    private static void AssertNoDependency(Assembly assembly, Assembly[] forbiddenAssemblies, string[] forbiddenNamespaces)
    {
        var forbidden = forbiddenAssemblies.Select(a => a.GetName().Name!).Concat(forbiddenNamespaces).ToArray();

        var result = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAny(forbidden).GetResult();

        Assert.True(
            result.IsSuccessful,
            $"{assembly.GetName().Name} dépend de {string.Join(", ", forbidden)} via : {string.Join(", ", result.FailingTypeNames ?? [])}");
    }
}
