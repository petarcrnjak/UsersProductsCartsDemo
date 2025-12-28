namespace AbySalto.Mid.Domain.Entities;

public sealed class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    private User() { }

    public User(string username, string email, string paswordHash, DateTime createdAt)
    {
        Username = username;
        Email = email;
        PasswordHash = paswordHash;
        CreatedAt = createdAt;
    }
}