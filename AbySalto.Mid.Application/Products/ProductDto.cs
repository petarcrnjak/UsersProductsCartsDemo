namespace AbySalto.Mid.Application.Products;

public sealed record ProductDto(
    int Id,
    string Title,
    string? Description,
    decimal Price,
    decimal? DiscountPercentage,
    decimal? Rating,
    int? Stock,
    string? Brand,
    string? Category,
    string? Thumbnail,
    List<string>? Images
 );
