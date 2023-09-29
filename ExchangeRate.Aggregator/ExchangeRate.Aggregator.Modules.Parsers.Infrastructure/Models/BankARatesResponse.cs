namespace ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Models;

public class BankARatesResponse
{
    public bool Success { get; set; }

    public long Timestamp { get; set; }

    public string Base { get; set; }

    public DateOnly Date { get; set; }
    
    public Dictionary<string, decimal> Rates { get; set; }
}