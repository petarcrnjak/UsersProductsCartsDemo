using AbySalto.Mid.Application.Products.Responses;
using AbySalto.Mid.Application.Services.External;
using System.Net.Http.Json;
using System.Text.Json;

namespace AbySalto.Mid.Infrastructure.Services.External;

public sealed class DummyJsonApiClient : IDummyJsonApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public DummyJsonApiClient(HttpClient httpClient, JsonSerializerOptions jsonOptions)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions;
    }

    public async Task<PagedProductsResponse?> GetProductsAsync(int limit = 30, int skip = 0, string? sortBy = null, string? orderBy = null)
    {
        var queryParts = new List<string>
        {
            $"limit={limit}",
            $"skip={skip}"
        };

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            queryParts.Add($"sortBy={Uri.EscapeDataString(sortBy)}");
        }
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            queryParts.Add($"order={Uri.EscapeDataString(orderBy)}");
        }

        var url = "/products?" + string.Join("&", queryParts);
        return await _httpClient.GetFromJsonAsync<PagedProductsResponse>(url, _jsonOptions);
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProductResponse>($"/products/{id}", _jsonOptions);
    }
}