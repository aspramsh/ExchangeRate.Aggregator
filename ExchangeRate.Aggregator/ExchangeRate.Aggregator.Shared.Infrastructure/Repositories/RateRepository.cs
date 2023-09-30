using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;

public class RateRepository : EntityFrameworkGenericRepository<Rate>, IRateRepository
{
    public RateRepository(ILoggerFactory loggerFactory, ExchangeRateDbContext dbContext) : base(loggerFactory, dbContext)
    {
    }
}