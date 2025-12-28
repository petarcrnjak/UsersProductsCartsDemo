using AbySalto.Mid.Application.Common.Exceptions;
using AbySalto.Mid.Application.Favorites;
using AbySalto.Mid.Application.Favorites.Mapper;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Infrastructure.Services;

public sealed class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _repository;
    private readonly IProductService _productService;

    public FavoriteService(
        IFavoriteRepository repository,
        IProductService productService)
    {
        _repository = repository;
        _productService = productService;
    }

    public async Task<FavoriteDto> AddFavoriteAsync(AddFavoriteCommand command)
    {
        //TODO user context
        int uid = 1;

        var product = await _productService.GetProductByIdAsync(command.ProductId);
        if (product is null)
            throw new NotFoundException($"Product with id {command.ProductId} not found.");

        var existing = (await _repository.GetByUserIdAsync(uid)).FirstOrDefault(f => f.ProductId == command.ProductId);
        if (existing is not null)
            return existing.ToDto();

        var favorite = new Favorite(uid, command.ProductId, product.Title, product.Price, DateTime.UtcNow);

        var added = await _repository.AddAsync(favorite);
        return added.ToDto();
    }
}