namespace ExchangeRate.Aggregator.Shared.Abstractions.Entities;

public class Bank
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ApiSettings { get; set; }
    
    public ICollection<Rate> Rates { get; set; }
}