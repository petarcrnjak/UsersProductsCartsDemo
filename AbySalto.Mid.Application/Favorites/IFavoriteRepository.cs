using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Favorites;

public interface IFavoriteRepository
{
    Task<Favorite> AddAsync(Favorite favorite, CancellationToken cancellation = default);
    Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId, CancellationToken cancellation = default);
}