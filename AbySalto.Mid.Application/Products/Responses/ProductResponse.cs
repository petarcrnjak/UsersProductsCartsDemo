namespace AbySalto.Mid.Application.Products.Responses;

public sealed record ProductResponse(
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
