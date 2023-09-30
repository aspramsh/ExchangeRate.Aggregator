using MediatR;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.EditRate;

// TODO: Add fluent validation
public class EditRateCommand : IRequest<RateDetailResponse>
{
    public long Id { get; set; }
    
    public decimal Value { get; set; }
}