namespace AbySalto.Mid.Domain.Entities;

public sealed class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    // navigation to principal
    public User User { get; set; } = null!;
}