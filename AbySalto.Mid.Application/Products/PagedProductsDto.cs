namespace AbySalto.Mid.Application.Products;

public sealed record PagedProductsDto(
    List<ProductDto> Products,
    int Total,
    int Skip,
    int Limit
   );
