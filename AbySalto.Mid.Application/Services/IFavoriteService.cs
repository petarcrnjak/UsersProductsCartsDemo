using AbySalto.Mid.Application.Favorites;

namespace AbySalto.Mid.Application.Services;

public interface IFavoriteService
{
    Task<FavoriteDto> AddFavoriteAsync(AddFavoriteCommand command);
}