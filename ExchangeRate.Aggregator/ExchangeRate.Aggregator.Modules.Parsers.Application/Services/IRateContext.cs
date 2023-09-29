namespace ExchangeRate.Aggregator.Modules.Parsers.Application.Services;

public interface IRateContext
{
    IRateService GetConcreteBankService(string name);
}