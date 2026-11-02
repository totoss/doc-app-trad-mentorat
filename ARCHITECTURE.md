# Squelette DDD et architecture hexagonale

Monolithe modulaire .NET 10 : un seul déployable (`HexaApp.Api`), des modules indépendants qui ont chacun leur domaine, leurs ports et leurs adapters. Le module `Catalog` est un exemple complet à copier pour créer les vrais modules.

## La règle

**Les dépendances pointent vers le domaine, jamais l'inverse.** Le domaine ne connaît ni EF Core, ni ASP.NET, ni aucun SDK. Les tests d'architecture (`tests/HexaApp.Architecture.Tests`) échouent si la règle est enfreinte.

```text
HexaApp.Api ──────────────► Catalog.Infrastructure ──► Catalog.Application ──► Catalog.Domain ──► SharedKernel
(composition, adapters       (adapters sortants :       (use cases = ports       (agrégats, value
 entrants HTTP)               EF Core, PostgreSQL)       entrants ; ports         objects, règles,
                                                         sortants)                événements)
```

| Couche | Contient | Dépend de |
| --- | --- | --- |
| `SharedKernel` | `Result<T>`, `Error`, `AggregateRoot<TId>`, `IDomainEvent`, `IDomainEventDispatcher`, `IClock` | rien |
| `<Module>.Domain` | agrégats, value objects, erreurs métier, événements | `SharedKernel` |
| `<Module>.Application` | use cases (un par intention), ports sortants (`IProductRepository`, `ICatalogUnitOfWork`) | `Domain` |
| `<Module>.Infrastructure` | adapters sortants : `DbContext` du module (schéma PostgreSQL dédié), repositories, migrations | `Application` |
| `Api` | racine de composition, endpoints Minimal API, horloge système, dispatcher d'événements | tout |

## Conventions

- **Use case** : une classe, une méthode `ExecuteAsync(input, ct)`, un `Result` en retour. Pas de médiateur : l'endpoint injecte le use case et l'appelle.
- **Erreurs** : les erreurs attendues sont des `Error` typées (`Validation`, `NotFound`, `Conflict`, `Forbidden`), traduites une seule fois en `ProblemDetails` (400, 404, 409, 403) par `ResultHttpExtensions`. Les exceptions sont réservées aux bugs et aux pannes.
- **Invariants** : dans le domaine. Un value object ne peut exister que valide (`ProductName.Create` renvoie un `Result`).
- **Transactions** : un use case = un `CommitAsync`. L'unité de travail du module publie les événements de domaine en mémoire, puis enregistre.
- **Persistance** : un `DbContext` par module, un schéma PostgreSQL par module, une table d'historique de migrations par schéma.
- **Temps** : toujours via `IClock`, jamais `DateTime.Now` dans le domaine.

## Démarrer

```bash
docker compose up -d
```

```bash
dotnet run --project src/HexaApp.Api
```

En développement, l'API applique les migrations au démarrage et expose OpenAPI sur `/openapi/v1.json`. Exemple : `POST http://localhost:5080/catalog/products` avec `{ "name": "Café" }`.

```bash
dotnet test
```

## Ajouter un module

1. Copier `src/Modules/Catalog` et `tests/HexaApp.Catalog.*`, renommer `Catalog` partout.
2. Créer un `CatalogModule` équivalent dans `src/HexaApp.Api/Modules/<Module>/` et l'appeler dans `Program.cs`.
3. Ajouter les assemblies du module dans `LayerDependencyTests`.
4. Pour qu'un module lise un autre module, créer un projet `<Module>.Contracts` (interfaces et DTO publics) : c'est la seule référence autorisée entre modules.
5. Générer la première migration :

```bash
dotnet ef migrations add InitialCreate --project src/Modules/<Module>/HexaApp.<Module>.Infrastructure --startup-project src/HexaApp.Api --output-dir Persistence/Migrations
```

## Utiliser comme template

```bash
dotnet new install .
```

```bash
dotnet new hexa-ddd -n MonProjet
```

`HexaApp` est remplacé par le nom choisi dans les fichiers, namespaces et noms de projets.
