using AbySalto.Mid.Application.Products;
using AbySalto.Mid.Application.Products.Mapper;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Application.Services.External;

namespace AbySalto.Mid.Infrastructure.Services;

public sealed class ProductService : IProductService
{
    private readonly IDummyJsonApiClient _apiClient;

    public ProductService(IDummyJsonApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PagedProductsDto?> GetProductsAsync(int limit = 30, int skip = 0, string? sortBy = null, string? orderBy = null)
    {
        var products = await _apiClient.GetProductsAsync(limit, skip, sortBy, orderBy);
        return products?.ToDto();
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _apiClient.GetProductByIdAsync(id);
        return product?.ToDto();
    }
}