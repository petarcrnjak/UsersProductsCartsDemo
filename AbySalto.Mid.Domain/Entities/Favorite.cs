namespace AbySalto.Mid.Domain.Entities;

public sealed class Favorite
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int ProductId { get; private set; }
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public DateTime AddedAt { get; private set; } = DateTime.UtcNow;

    // navigation: a Favorite belongs to a single User
    public User User { get; set; } = null!;

    public Favorite(int userId, int productId, string title, decimal price, DateTime addedAt)
    {
        UserId = userId;
        ProductId = productId;
        Title = title;
        Price = price;
        AddedAt = addedAt;
    }

}