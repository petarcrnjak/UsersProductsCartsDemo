namespace AbySalto.Mid.Infrastructure.Configuration;

public sealed class ExternalApisSettings
{
    public const string SectionName = "ExternalApis";

    public string BaseUrl { get; set; } = string.Empty;
}