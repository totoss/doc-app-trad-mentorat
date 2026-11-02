using HexaApp.Catalog.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HexaApp.Catalog.Infrastructure.Persistence;

/// <summary>Le mapping reste hors du domaine : les value objects passent par des conversions.</summary>
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new ProductId(value))
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasConversion(name => name.Value, value => ProductName.Create(value).Value)
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Ignore(p => p.DomainEvents);
    }
}
