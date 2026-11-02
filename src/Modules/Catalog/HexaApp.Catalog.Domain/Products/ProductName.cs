using HexaApp.SharedKernel;

namespace HexaApp.Catalog.Domain.Products;

/// <summary>Value object : un nom de produit ne peut exister que valide.</summary>
public sealed record ProductName
{
    public const int MaxLength = 100;

    private ProductName(string value) => Value = value;

    public string Value { get; }

    public static Result<ProductName> Create(string? value)
    {
        var trimmed = value?.Trim();
        if (string.IsNullOrEmpty(trimmed))
        {
            return ProductErrors.NameRequired;
        }

        if (trimmed.Length > MaxLength)
        {
            return ProductErrors.NameTooLong;
        }

        return new ProductName(trimmed);
    }

    public override string ToString() => Value;
}
