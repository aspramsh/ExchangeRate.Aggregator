using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;
using MapsterMapper;
using MediatR;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Queries.GetRateById;

public class GetRateHandler : IRequestHandler<GetRateQuery, RateDetailResponse>
{
    private readonly IMapper _mapper;
    
    private readonly IRateRepository _rateRepository;
    
    public GetRateHandler(
        IMapper mapper,
        IRateRepository rateRepository)
    {
        _mapper = mapper;
        _rateRepository = rateRepository;
    }

    private static Func<IIncludable<Rate>, IIncludable> Included =>
        x => x.Include(i => i.Currency)
            .Include(i => i.BaseCurrency)
            .Include(i => i.Bank);
    
    public async Task<RateDetailResponse> Handle(GetRateQuery request, CancellationToken cancellationToken)
    {
        var result =
            await _rateRepository.GetOneAsync(x => x.Id == request.Id, Included, cancellationToken: cancellationToken);

        return _mapper.Map<RateDetailResponse>(result);
    }
}