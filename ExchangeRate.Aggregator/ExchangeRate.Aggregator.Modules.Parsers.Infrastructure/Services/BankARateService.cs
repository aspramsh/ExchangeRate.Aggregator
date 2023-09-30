using ExchangeRate.Aggregator.Modules.Parsers.Application.Services;
using ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Models;
using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Services;

public class BankARateService : IRateService
{
    public const string Name = "Bank A";
    
    private readonly IHttpClientFactory _httpClientFactory;
    
    private readonly ICurrencyRepository _currencyRepository;

    private readonly IRateRepository _rateRepository;
    
    public BankARateService(
        IHttpClientFactory httpClientFactory,
        ICurrencyRepository currencyRepository,
        IRateRepository rateRepository)
    {
        _httpClientFactory = httpClientFactory;
        _currencyRepository = currencyRepository;
        _rateRepository = rateRepository;
    }

    public async Task GetLatestRatesAsync(Bank bankSettings, string baseCurrency = "EUR", CancellationToken cancellationToken = default)
    {
        var existingBase =
            await _currencyRepository.GetOneAsync(x => x.Code == baseCurrency, cancellationToken: cancellationToken);
        
        try
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(JObject.Parse(bankSettings.ApiSettings).First?.Value<JProperty>().Value.ToString() ?? string.Empty),
                Method = HttpMethod.Get
            };

            var response = await SendRequestAsync(httpRequestMessage);

            var responseStr = await response.Content.ReadAsStringAsync(cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseObj = JsonConvert.DeserializeObject<BankARatesResponse>(responseStr);

            // TODO: create currency seed list
            var currencyCodes = responseObj.Rates.Select(x => x.Key);

            var existingCurrencies = await _currencyRepository.FindByAsync(x => currencyCodes.Contains(x.Code),
                cancellationToken: cancellationToken);

            var existingCodes = existingCurrencies.Select(x => x.Code);

            var newCodes = currencyCodes.Except(existingCodes);

            var entities = newCodes.Select(x => new Currency
            {
                Code = x,
                Name = x
            });

            await _currencyRepository.InsertRangeAsync(entities.ToList(), cancellationToken);
            await _currencyRepository.SaveChangesAsync(cancellationToken);

            var currencies = await _currencyRepository.FindByAsync(x => true, cancellationToken: cancellationToken);

            var result = responseObj.Rates.Select(r => new Rate
            {
                BankId = bankSettings.Id,
                BaseCurrencyId = existingBase.Id,
                CurrencyId = currencies.FirstOrDefault(x => x.Code == r.Key)?.Id ?? 0,
                DateTime = DateTimeOffset.FromUnixTimeSeconds(responseObj.Timestamp),
                Value = r.Value,
            });

            await _rateRepository.InsertRangeAsync(result.ToList(), cancellationToken);
            await _rateRepository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // TODO: log here
            throw;
        }
    }
    
    private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
    {
        var httpClient = _httpClientFactory.CreateClient(HttpClientConstants.ParserClient);

        var response = await httpClient.SendAsync(request);

        return response;
    }

}