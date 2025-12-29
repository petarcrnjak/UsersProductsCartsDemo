using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Favorites.Mapper;

public static class FavoriteMapper
{
    public static FavoriteDto ToDto(this Favorite favorite, string userName)
    {
        if (favorite is null)
            return null!;

        return new FavoriteDto(
            favorite.Id,
            userName,
            favorite.ProductId,
            favorite.Title,
            favorite.Price,
            favorite.AddedAt
        );
    }
}
