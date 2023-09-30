using ExchangeRate.Aggregator.Shared.Abstractions.Modules;

namespace ExchangeRate.Aggregator.Modules.Rates.Api;

public class RatesModule : IModule
{
    public string Name { get; } = "Rates";

    public IEnumerable<string> Policies { get; } = new[]
    {
        "rates",
    };

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAggregatorsModule(configuration);
    }

    public void Use(IApplicationBuilder app)
    {
    }
}