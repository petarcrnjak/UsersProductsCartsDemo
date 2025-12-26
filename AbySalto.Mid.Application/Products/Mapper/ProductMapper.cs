using AbySalto.Mid.Application.Products.Responses;

namespace AbySalto.Mid.Application.Products.Mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(this ProductResponse src)
    {
        if (src is null)
            return null!; // caller should handle nulls; keep signature simple

        return new ProductDto(
            src.Id,
            src.Title,
            src.Description,
            src.Price,
            src.DiscountPercentage,
            src.Rating,
            src.Stock,
            src.Brand,
            src.Category,
            src.Thumbnail,
            src.Images
        );
    }

    public static PagedProductsDto ToDto(this PagedProductsResponse src)
    {
        if (src is null)
            return null!;

        var products = src.Products?.Select(p => p.ToDto()).ToList() ?? [];

        return new PagedProductsDto(products, src.Total, src.Skip, src.Limit);
    }
}
