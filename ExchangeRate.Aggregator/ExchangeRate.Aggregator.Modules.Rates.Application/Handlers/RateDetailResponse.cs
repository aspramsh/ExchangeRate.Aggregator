using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.ResponseModels;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers;

public class RateDetailResponse
{
    public long Id { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public decimal Value { get; set; }

    public int BaseCurrencyId { get; set; }

    public CurrencyResponse BaseCurrency { get; set; }
    
    public int CurrencyId { get; set; }
    
    public CurrencyResponse Currency { get; set; }
    
    public int BankId { get; set; }
    
    public BankResponse Bank { get; set; }
}