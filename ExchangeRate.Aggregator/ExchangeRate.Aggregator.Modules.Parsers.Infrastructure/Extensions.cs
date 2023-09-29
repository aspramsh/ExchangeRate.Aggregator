using System.Runtime.CompilerServices;
using ExchangeRate.Aggregator.Modules.Parsers.Application.Repositories;
using ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("ExchangeRate.Aggregator.Modules.Parsers")]

namespace ExchangeRate.Aggregator.Modules.Parsers.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddParsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IBankRepository, BankRepository>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPostgresContext<ExchangeRateDbContext>();
        
        var context = services.BuildServiceProvider().GetRequiredService<ExchangeRateDbContext>();    
        context.Database.Migrate();
        
        return services;
    }
    
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        return services;
    }
}