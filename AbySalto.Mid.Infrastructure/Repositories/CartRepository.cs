using AbySalto.Mid.Application.Carts;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellation = default)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .FirstAsync(c => c.UserId == userId, cancellation);
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellation = default)
    {
        await _context.Carts.AddAsync(cart, cancellation);
        await _context.SaveChangesAsync(cancellation);
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellation = default)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync(cancellation);
        return cart;
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId, CancellationToken cancellation = default)
    {
        return await _context.CartItems
            .AsNoTracking()
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId, cancellation);
    }

    public async Task RemoveCartItemAsync(int cartItemId, CancellationToken cancellation = default)
    {
        var item = await _context.CartItems.FindAsync(new object[] { cartItemId }, cancellation);
        if (item is not null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync(cancellation);
        }
    }

    public async Task<Cart> GetOrCreateByUserIdAsync(int userId, CancellationToken cancellation = default)
    {
        var cart = await _context.Carts
            .AsNoTracking()
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellation);

        if (cart is not null)
            return cart;

        var newCart = new Cart
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        return await CreateAsync(newCart, cancellation);
    }
}
