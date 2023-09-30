namespace ExchangeRate.Aggregator.Shared.Abstractions.Entities;

public class Currency
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public ICollection<Rate> Rates { get; set; }
    
    public ICollection<Rate> BaseRates { get; set; }
}