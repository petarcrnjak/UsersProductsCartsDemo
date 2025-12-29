using AbySalto.Mid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbySalto.Mid.Infrastructure.Persistence.Configuration;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ci => ci.Price)
            .HasPrecision(18, 2);

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.HasIndex(ci => ci.CartId);

        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}