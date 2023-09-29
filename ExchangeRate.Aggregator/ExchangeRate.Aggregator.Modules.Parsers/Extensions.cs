using System.Runtime.CompilerServices;
using ExchangeRate.Aggregator.Modules.Parsers.Application;
using ExchangeRate.Aggregator.Modules.Parsers.Infrastructure;

[assembly:InternalsVisibleTo("ExchangeRate.Aggregator.Bootstrapper")]

namespace ExchangeRate.Aggregator.Modules.Parsers;

internal static class Extensions
{
    public static IServiceCollection AddParsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddParsersInfrastructure(configuration);
        services.AddApplication();
        
        return services;
    }
}