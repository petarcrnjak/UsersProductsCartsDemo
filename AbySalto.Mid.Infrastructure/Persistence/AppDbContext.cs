using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Favorite> Favorites { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all IEntityTypeConfiguration<> implementations that live in the persistence configurations assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartConfiguration).Assembly);

        // Explicitly ensure known configurations are registered (keeps intent clear and avoids missed registrations)
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
    }
}