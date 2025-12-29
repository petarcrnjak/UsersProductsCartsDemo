using AbySalto.Mid.Application.Products;
using AbySalto.Mid.Application.Products.Mapper;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Application.Services.External;
using AbySalto.Mid.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AbySalto.Mid.Infrastructure.Services;

public sealed class ProductService : IProductService
{
    private readonly IDummyJsonApiClient _apiClient;
    private readonly IMemoryCache _cache;
    private readonly IOptionsMonitor<CacheSettings> _cacheOptions;

    public ProductService(IDummyJsonApiClient apiClient, IMemoryCache cache, IOptionsMonitor<CacheSettings> cacheOptions)
    {
        _apiClient = apiClient;
        _cache = cache;
        _cacheOptions = cacheOptions;
    }

    public async Task<PagedProductsDto?> GetProductsAsync(int limit = 30, int skip = 0, string? sortBy = null, string? orderBy = null)
    {
        // Use a cache key that includes pagination and sort parameters
        var key = $"products:limit={limit}:skip={skip}:sortBy={sortBy ?? ""}:order={orderBy ?? ""}";

        return await _cache.GetOrCreateAsync<PagedProductsDto?>(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheOptions.CurrentValue.ProductsPageMinutes);
            entry.Priority = CacheItemPriority.Normal;

            var external = await _apiClient.GetProductsAsync(limit, skip, sortBy, orderBy);
            return external?.ToDto();
        });
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var key = $"product:{id}";

        return await _cache.GetOrCreateAsync<ProductDto?>(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheOptions.CurrentValue.ProductDetailMinutes);
            entry.Priority = CacheItemPriority.High;

            var external = await _apiClient.GetProductByIdAsync(id);
            return external?.ToDto();
        });
    }
}