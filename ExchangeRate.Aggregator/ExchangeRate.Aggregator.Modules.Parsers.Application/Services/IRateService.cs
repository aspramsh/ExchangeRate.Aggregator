using ExchangeRate.Aggregator.Shared.Infrastructure.Entities;

namespace ExchangeRate.Aggregator.Modules.Parsers.Application.Services;

public interface IRateService
{
    Task GetLatestRatesAsync(Bank bankSettings, string baseCurrency = "EUR",
        CancellationToken cancellationToken = default);
}