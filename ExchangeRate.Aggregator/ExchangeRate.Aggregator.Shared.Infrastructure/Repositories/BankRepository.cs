using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.DbContexts;
using Microsoft.Extensions.Logging;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;

public class BankRepository : EntityFrameworkGenericRepository<Bank>, IBankRepository
{
    public BankRepository(ILoggerFactory loggerFactory, ExchangeRateDbContext dbContext)
        : base(loggerFactory, dbContext)
    {
    }
}