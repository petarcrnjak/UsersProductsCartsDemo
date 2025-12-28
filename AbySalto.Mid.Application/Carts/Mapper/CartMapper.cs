using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Carts.Mapper;

public static class CartMapper
{
    public static CartDto ToDto(this Cart cart, string username)
    {
        var items = cart.CartItems.Select(ToDto).ToList();

        return new CartDto(
            username,
            items,
            cart.CreatedAt
        );
    }

    public static CartItemDto ToDto(this CartItem item)
    {
        return new CartItemDto(
            item.ProductId,
            item.Title,
            item.Price,
            item.Quantity
        );
    }
}
