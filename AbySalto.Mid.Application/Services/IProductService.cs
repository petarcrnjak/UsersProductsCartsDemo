using AbySalto.Mid.Application.Products;

namespace AbySalto.Mid.Application.Services;

public interface IProductService
{
    Task<PagedProductsDto?> GetProductsAsync(int limit = 30, int skip = 0, string? sortBy = null, string? orderBy = null);
    Task<ProductDto?> GetProductByIdAsync(int id);
}