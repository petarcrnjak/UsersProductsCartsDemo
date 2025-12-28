namespace AbySalto.Mid.Domain.Entities;

public sealed class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // One cart per user
    public Cart? Cart { get; set; }

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    private User() { }

    public User(string username, string email, string passwordHash, DateTime createdAt)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }
}