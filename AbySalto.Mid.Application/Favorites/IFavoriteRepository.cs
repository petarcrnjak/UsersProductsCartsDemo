using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Favorites;

public interface IFavoriteRepository
{
    Task<Favorite> AddAsync(Favorite favorite, CancellationToken cancellation = default);
    Task<Favorite?> GetByUserIdAndProductIdAsync(int userId, int productId, CancellationToken cancellation = default);
}