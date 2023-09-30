using ExchangeRate.Aggregator.Shared.Abstractions.Entities;

namespace ExchangeRate.Aggregator.Modules.Parsers.Application.Services;

public interface IRateService
{
    Task GetLatestRatesAsync(Bank bankSettings, string baseCurrency = "EUR",
        CancellationToken cancellationToken = default);
}