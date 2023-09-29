using ExchangeRate.Aggregator.Modules.Parsers.Application.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using ExchangeRate.Aggregator.Shared.Infrastructure.Entities;
using ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Repositories;

public class RateRepository : EntityFrameworkGenericRepository<Rate>, IRateRepository
{
    public RateRepository(ILoggerFactory loggerFactory, ExchangeRateDbContext dbContext) : base(loggerFactory, dbContext)
    {
    }
}