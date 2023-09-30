using System.Runtime.CompilerServices;
using ExchangeRate.Aggregator.Modules.Rates.Application;
using ExchangeRate.Aggregator.Modules.Rates.Infrastructure;
using ExchangeRate.Aggregator.Shared.Infrastructure;

[assembly:InternalsVisibleTo("ExchangeRate.Aggregator.Bootstrapper")]

namespace ExchangeRate.Aggregator.Modules.Rates.Api;

internal static class Extensions
{
    public static IServiceCollection AddAggregatorsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApplication()
            .AddInfrastructure(configuration);
        
        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddValidationBehaviours()
            .AddVersioning()
            .AddAggregatorsInfrastructure(configuration);
    }
}