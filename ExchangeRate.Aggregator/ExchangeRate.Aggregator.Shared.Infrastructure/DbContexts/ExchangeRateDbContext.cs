using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Infrastructure.Configurations;
using ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;

public class ExchangeRateDbContext : DbContext
{
    public ExchangeRateDbContext(DbContextOptions<ExchangeRateDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Bank> Bank { get; set; }
    
    public DbSet<Currency> Currency { get; set; }
    
    public DbSet<Rate> Rate { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BankConfiguration());
        builder.ApplyConfiguration(new CurrencyConfiguration());
        builder.ApplyConfiguration(new RateConfiguration());

        builder.PostgresModelCreating();
    }
}