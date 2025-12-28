namespace AbySalto.Mid.Domain.Entities;

public sealed class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // user can have many carts
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}