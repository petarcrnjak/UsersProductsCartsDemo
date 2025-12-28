using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Application.Auth.Interfaces;
using AbySalto.Mid.Application.Carts;
using AbySalto.Mid.Application.Carts.Mapper;
using AbySalto.Mid.Application.Common.Exceptions;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Infrastructure.Services;

public sealed class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductService _productService;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public CartService(
        ICartRepository cartRepository,
        IProductService productService, IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _cartRepository = cartRepository;
        _productService = productService;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CartDto> GetCartAsync(CancellationToken cancellation = default)
    {
        var userId = _currentUserService.GetUserId();

        var user = await _userRepository.GetByIdAsync(userId, cancellation)
                  ?? throw new NotFoundException("User not found.");

        var cart = await _cartRepository.GetByUserIdAsync(userId, cancellation)
                  ?? throw new NotFoundException("Cart not found.");

        return cart.ToDto(user.Username);
    }

    public async Task<CartDto> AddToCartAsync(AddToCartCommand command, CancellationToken cancellation = default)
    {
        var userId = _currentUserService.GetUserId();
        var user = await EnsureUserExistsAsync(userId, cancellation);

        CartValidator.ValidateQuantity(command);

        // Validate product exists
        var product = await _productService.GetProductByIdAsync(command.ProductId)
                ?? throw new NotFoundException($"Product with id {command.ProductId} not found.");

        // Get or create cart (extracted helper)
        var cart = await _cartRepository.GetOrCreateByUserIdAsync(userId, cancellation);

        // Check if item already in cart
        var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, command.ProductId, cancellation);
        if (existingItem is not null)
        {
            // Update quantity
            existingItem.Quantity += command.Quantity;
        }
        else
        {
            // Add new item
            var newItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = command.ProductId,
                Title = product.Title,
                Price = product.Price,
                Quantity = command.Quantity
            };
            cart.CartItems.Add(newItem);
        }

        // persist changes and use the tracked entity returned from repo
        cart = await _cartRepository.UpdateAsync(cart, cancellation);
        return cart.ToDto(user.Username);
    }

    public async Task<CartDto> RemoveFromCartAsync(int productId, CancellationToken cancellation = default)
    {
        var userId = _currentUserService.GetUserId();
        var user = await EnsureUserExistsAsync(userId, cancellation);

        var cart = await _cartRepository.GetByUserIdAsync(userId, cancellation)
                   ?? throw new NotFoundException("Cart not found.");

        var item = await _cartRepository.GetCartItemAsync(cart.Id, productId, cancellation)
                   ?? throw new NotFoundException($"Product with id {productId} not found in cart.");

        await _cartRepository.RemoveCartItemAsync(item.Id, cancellation);

        // Reload cart
        return cart!.ToDto(user.Username);
    }

    private async Task<User> EnsureUserExistsAsync(int userId, CancellationToken cancellation = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        return user ?? throw new NotFoundException("User not found.");
    }
}
