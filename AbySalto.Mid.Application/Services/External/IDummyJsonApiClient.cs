using AbySalto.Mid.Application.Products.Responses;

namespace AbySalto.Mid.Application.Services.External;

public interface IDummyJsonApiClient
{
    Task<PagedProductsResponse?> GetProductsAsync(int limit = 30, int skip = 0, string? sortBy = null, string? orderBy = null);
    Task<ProductResponse?> GetProductByIdAsync(int id);
}