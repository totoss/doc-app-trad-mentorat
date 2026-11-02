using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Domain.Products;

public static class ProductErrors
{
    public static readonly Error NameRequired =
        Error.Validation("product.name_required", "Le nom du produit est obligatoire.");

    public static readonly Error NameTooLong =
        Error.Validation("product.name_too_long", $"Le nom du produit dépasse {ProductName.MaxLength} caractères.");

    public static readonly Error AlreadyPublished =
        Error.Conflict("product.already_published", "Le produit est déjà publié.");

    public static Error NotFound(ProductId id) =>
        Error.NotFound("product.not_found", $"Aucun produit avec l'identifiant {id}.");
}
