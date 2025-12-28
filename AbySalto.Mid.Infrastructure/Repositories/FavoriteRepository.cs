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
        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync(cancellation);
        return favorite;
    }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId, CancellationToken cancellation = default)
    {
        return await _context.Favorites
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.AddedAt)
            .ToListAsync(cancellation);
    }
}