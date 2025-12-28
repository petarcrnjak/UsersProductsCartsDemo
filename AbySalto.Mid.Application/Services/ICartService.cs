using AbySalto.Mid.Application.Carts;

namespace AbySalto.Mid.Application.Services;

public interface ICartService
{
    Task<CartDto> GetCartAsync(CancellationToken cancellation = default);
    Task<CartDto> AddToCartAsync(AddToCartCommand command, CancellationToken cancellation = default);
    Task<CartDto> RemoveFromCartAsync(int productId, CancellationToken cancellation = default);
}
