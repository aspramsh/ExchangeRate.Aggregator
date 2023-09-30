using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Aggregator.Modules.Rates.Application;

public static class Extensions
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