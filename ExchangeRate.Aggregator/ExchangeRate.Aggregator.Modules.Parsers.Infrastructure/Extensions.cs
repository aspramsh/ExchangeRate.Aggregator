using System.Runtime.CompilerServices;
using ExchangeRate.Aggregator.Modules.Parsers.Application.Repositories;
using ExchangeRate.Aggregator.Modules.Parsers.Application.Services;
using ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Repositories;
using ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Services;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using ExchangeRate.Aggregator.Shared.Infrastructure.Helpers;
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
        services.AddServices();
        
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IBankRepository, BankRepository>();
        
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        
        services.AddScoped<IRateRepository, RateRepository>();
        
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

    private static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IRateService, BankARateService>();
        
        services.AddScoped<IRateContext, RateContext>();

        services.AddHttpClient(HttpClientConstants.ParserClient);
        
        return services;
    }
}