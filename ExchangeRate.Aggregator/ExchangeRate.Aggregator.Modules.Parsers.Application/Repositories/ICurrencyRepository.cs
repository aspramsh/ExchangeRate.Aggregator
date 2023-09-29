using ExchangeRate.Aggregator.Shared.Infrastructure.Entities;
using ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;

namespace ExchangeRate.Aggregator.Modules.Parsers.Application.Repositories;

public interface ICurrencyRepository : IEntityFrameworkGenericRepository<Currency>
{
}