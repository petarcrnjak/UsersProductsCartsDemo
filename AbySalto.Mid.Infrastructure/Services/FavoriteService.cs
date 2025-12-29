using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Application.Auth.Interfaces;
using AbySalto.Mid.Application.Common.Exceptions;
using AbySalto.Mid.Application.Favorites;
using AbySalto.Mid.Application.Favorites.Mapper;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Infrastructure.Services;

public sealed class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IProductService _productService;
    private readonly ICurrentUserService _currentUserService;

    public FavoriteService(
        IFavoriteRepository favoriteRepository,
        IProductService productService,
        ICurrentUserService currentUserService,
        IUserRepository userRepository)
    {
        _favoriteRepository = favoriteRepository;
        _productService = productService;
        _currentUserService = currentUserService;
    }

    public async Task<FavoriteDto> AddFavoriteAsync(AddFavoriteCommand command)
    {
        var userId = _currentUserService.GetUserId();

        var product = await _productService.GetProductByIdAsync(command.ProductId)
                ?? throw new NotFoundException($"Product with id {command.ProductId} not found.");

        var favorite = new Favorite(userId, command.ProductId, product.Title, product.Price, DateTime.UtcNow);
        var added = await _favoriteRepository.AddAsync(favorite);

        var username = await _currentUserService.GetUsernameAsync() ?? string.Empty;
        return added.ToDto(username);
    }
}