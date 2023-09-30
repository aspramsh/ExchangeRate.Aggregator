using System.Reflection;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;
using ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Aggregator.Modules.Rates.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddAggregatorsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMappings();
        
        services.AddDatabase(configuration);

        services.AddRepositories();
        
        return services;
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IRateRepository, RateRepository>();

        return services;
    }
    
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPostgresContext<ExchangeRateDbContext>();
        
        var context = services.BuildServiceProvider().GetRequiredService<ExchangeRateDbContext>();    
        context.Database.Migrate();
        
        return services;
    }
    
    private static void AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}