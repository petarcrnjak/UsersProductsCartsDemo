using AbySalto.Mid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbySalto.Mid.Infrastructure.Persistence.Configurations;

public sealed class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(f => f.Price)
               .HasPrecision(18, 2);

        builder.Property(f => f.AddedAt)
               .IsRequired();

        // Composite unique index to prevent duplicate favorites per user/product
        builder.HasIndex(f => new { f.UserId, f.ProductId })
               .IsUnique();

        builder.HasOne(f => f.User)
               .WithMany(u => u.Favorites)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}