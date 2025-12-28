using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Carts;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellation = default);
    Task<Cart> CreateAsync(Cart cart, CancellationToken cancellation = default);
    Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellation = default);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId, CancellationToken cancellation = default);
    Task RemoveCartItemAsync(int cartItemId, CancellationToken cancellation = default);
    Task<Cart> GetOrCreateByUserIdAsync(int userId, CancellationToken cancellation = default);
}
