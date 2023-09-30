using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;

public class CurrencyRepository : EntityFrameworkGenericRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(ILoggerFactory loggerFactory, ExchangeRateDbContext dbContext) : base(loggerFactory, dbContext)
    {
    }
}