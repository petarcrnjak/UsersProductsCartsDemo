namespace AbySalto.Mid.Application.Carts;

public sealed record CartDto
{
    public string Username { get; init; }
    public List<CartItemDto> Items { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal Total => Items.Sum(i => i.Total);

    public CartDto(string username, List<CartItemDto> items, DateTime createdAt)
    {
        Username = username;
        Items = items;
        CreatedAt = createdAt;
    }
}
