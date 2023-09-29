using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;

public class ExchangeRateDbContextFactory : IDesignTimeDbContextFactory<ExchangeRateDbContext>
{
    public ExchangeRateDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ExchangeRateDbContext>();
        
        // TODO: get from appsettings
        builder.UseNpgsql("Server=localhost;Port=5432;Database=rate_aggregator;Username=postgres;Password=Aspram#;SslMode=Prefer;Include Error Detail=true;");

        return (ExchangeRateDbContext)Activator.CreateInstance(typeof(ExchangeRateDbContext), builder.Options);
    }
}