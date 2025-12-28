namespace AbySalto.Mid.Infrastructure.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "ConnectionStrings";
    public string? DefaultConnection { get; set; } = string.Empty;
}
