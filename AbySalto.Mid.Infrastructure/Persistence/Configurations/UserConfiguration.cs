using AbySalto.Mid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AbySalto.Mid.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();
        builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();

        // One-to-one: User -> Cart
        builder.HasOne(u => u.Cart)
               .WithOne(c => c.User)
               .HasForeignKey<Cart>(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Favorites)
               .WithOne(f => f.User)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}