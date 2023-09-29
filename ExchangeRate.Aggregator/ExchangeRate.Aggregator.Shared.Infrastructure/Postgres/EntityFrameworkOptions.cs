namespace ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;

public class EntityFrameworkOptions
{
    public bool Enabled { get; set; }

    public IDictionary<string, string> Connections { get; set; } = new Dictionary<string, string>();

    public bool EnableSensitiveDataLogging { get; set; } = true;

    public bool EnableDetailedErrors { get; set; } = true;

    public bool UseQueryTrackingBehavior { get; set; } = true;

    public bool EnableRetryOnFailure { get; set; } = true;

    public bool EnableServiceProviderCaching { get; set; } = true;
}