using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;
using ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;
using MapsterMapper;
using MediatR;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.EditRate;

public class EditRateHandler : IRequestHandler<EditRateCommand, RateDetailResponse>
{
    private readonly IMapper _mapper;
    
    private readonly IRateRepository _rateRepository;
    
    public EditRateHandler(
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
    
    public async Task<RateDetailResponse> Handle(EditRateCommand request, CancellationToken cancellationToken)
    {
       var existing = await _rateRepository.GetOneAsync(x => x.Id == request.Id, isTracked: true,
            cancellationToken: cancellationToken);
        
        if (existing == default)
        {
            throw new NotFoundException(new ErrorModel
            {
                Message = "Rate does not exist"
            });
        }

        existing.Value = request.Value;

        await _rateRepository.SaveChangesAsync(cancellationToken);
            
        var result =
            await _rateRepository.GetOneAsync(x => x.Id == request.Id, Included, cancellationToken: cancellationToken);

        return _mapper.Map<RateDetailResponse>(result);
    }
}