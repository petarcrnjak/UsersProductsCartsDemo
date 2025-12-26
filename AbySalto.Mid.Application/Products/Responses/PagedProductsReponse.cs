namespace AbySalto.Mid.Application.Products.Responses;

public sealed record PagedProductsResponse(
    List<ProductResponse> Products,
    int Total,
    int Skip,
    int Limit
);
