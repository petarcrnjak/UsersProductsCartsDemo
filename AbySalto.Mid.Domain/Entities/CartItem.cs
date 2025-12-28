namespace AbySalto.Mid.Domain.Entities;

public sealed class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public required string Title { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    // navigation
    public Cart Cart { get; set; } = null!;
}