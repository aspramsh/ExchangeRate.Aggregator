using System.Reflection;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


[assembly: InternalsVisibleTo("ExchangeRate.Aggregator.Modules.Parsers")]
[assembly: InternalsVisibleTo("ExchangeRate.Aggregator.Modules.Parsers.Infrastructure")]

namespace ExchangeRate.Aggregator.Modules.Parsers.Application;

internal static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.RegisterHandlers();

        return services;
    }
    
    private static void RegisterHandlers(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}