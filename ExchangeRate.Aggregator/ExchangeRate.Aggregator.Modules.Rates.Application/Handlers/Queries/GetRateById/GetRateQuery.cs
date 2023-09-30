using MediatR;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Queries.GetRateById;

public class GetRateQuery : IRequest<RateDetailResponse>
{
    public long Id { get; set; }
}