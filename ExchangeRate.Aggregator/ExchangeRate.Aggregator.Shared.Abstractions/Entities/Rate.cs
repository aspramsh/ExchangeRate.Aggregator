namespace ExchangeRate.Aggregator.Shared.Abstractions.Entities;

public class Rate
{
    public long Id { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public decimal Value { get; set; }

    public int BaseCurrencyId { get; set; }

    public Currency BaseCurrency { get; set; }
    
    public int CurrencyId { get; set; }
    
    public Currency Currency { get; set; }
    
    public int BankId { get; set; }
    
    public Bank Bank { get; set; }
}