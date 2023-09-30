using MediatR;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.CreateRate;

// TODO: Add fluent validation
public class CreateRateCommand : IRequest<RateDetailResponse>
{
    public decimal Value { get; set; }

    public int BaseCurrencyId { get; set; }

    public int CurrencyId { get; set; }
    
    public int BankId { get; set; }
}