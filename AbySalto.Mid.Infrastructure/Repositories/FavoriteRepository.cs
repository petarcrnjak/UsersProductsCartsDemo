using AbySalto.Mid.Application.Common.Exceptions;
using AbySalto.Mid.Application.Favorites;
using AbySalto.Mid.Domain.Entities;
using AbySalto.Mid.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Mid.Infrastructure.Repositories;

internal sealed class FavoriteRepository : IFavoriteRepository
{
    private readonly AppDbContext _context;

    public FavoriteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Favorite> AddAsync(Favorite favorite, CancellationToken cancellation = default)
    {
        await _context.Favorites.AddAsync(favorite, cancellation);
        try
        {
            await _context.SaveChangesAsync(cancellation);
            return favorite;
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            throw new ConflictException($"Favorite for product {favorite.ProductId} already exists.");
        }

    }

    public async Task<Favorite?> GetByUserIdAndProductIdAsync(int userId, int productId, CancellationToken cancellation = default)
    {
        return await _context.Favorites
           .AsNoTracking()
           .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId, cancellation);
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        var msg = ex.InnerException?.Message ?? ex.Message;
        return msg.Contains("duplicate key", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("unique constraint", StringComparison.OrdinalIgnoreCase);
    }
}