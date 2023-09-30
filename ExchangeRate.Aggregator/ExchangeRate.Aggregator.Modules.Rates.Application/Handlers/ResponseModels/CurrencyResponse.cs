namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.ResponseModels;

public class CurrencyResponse
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}