namespace AbySalto.Mid.Application.Carts;

public sealed record CartItemDto
{
    //public int Id { get; init; }
    public int ProductId { get; init; }
    public string? Title { get; init; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;

    public CartItemDto()
    {
    }

    public CartItemDto(int productId, string title, decimal price, int quantity)
    {
        //Id = id;
        ProductId = productId;
        Title = title;
        Price = price;
        Quantity = quantity;
    }
}
