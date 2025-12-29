namespace AbySalto.Mid.Infrastructure.Configuration;

public sealed class CacheSettings
{
    public const string SectionName = "CacheSettings";

    public int ProductsPageMinutes { get; set; } = 5;
    public int ProductDetailMinutes { get; set; } = 30;
}
