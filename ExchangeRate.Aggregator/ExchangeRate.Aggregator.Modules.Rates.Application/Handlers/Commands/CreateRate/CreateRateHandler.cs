using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;
using ExchangeRate.Aggregator.Shared.Infrastructure.Repositories;
using MapsterMapper;
using MediatR;

namespace ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.CreateRate;

public class CreateRateHandler : IRequestHandler<CreateRateCommand, RateDetailResponse>
{
    private readonly IMapper _mapper;
    
    private readonly IBankRepository _bankRepository;
    
    private readonly ICurrencyRepository _currencyRepository;
    
    private readonly IRateRepository _rateRepository;
    
    public CreateRateHandler(
        IMapper mapper,
        IBankRepository bankRepository,
        ICurrencyRepository currencyRepository,
        IRateRepository rateRepository)
    {
        _mapper = mapper;
        _bankRepository = bankRepository;
        _currencyRepository = currencyRepository;
        _rateRepository = rateRepository;
    }
    
    private static Func<IIncludable<Rate>, IIncludable> Included =>
        x => x.Include(i => i.Currency)
            .Include(i => i.BaseCurrency)
            .Include(i => i.Bank);
    
    public async Task<RateDetailResponse> Handle(CreateRateCommand request, CancellationToken cancellationToken)
    {
        var bank = await _bankRepository.GetOneAsync(x => x.Id == request.BankId, cancellationToken: cancellationToken);

        if (bank == default)
        {
            throw new NotFoundException(new ErrorModel
            {
                Message = "Bank does not exist"
            });
        }

        var baseCurrency = await _currencyRepository.GetOneAsync(x => x.Id == request.BaseCurrencyId,
            cancellationToken: cancellationToken);
        
        if (baseCurrency == default)
        {
            throw new NotFoundException(new ErrorModel
            {
                Message = "Base currency does not exist"
            });
        }
        
        var currency = await _currencyRepository.GetOneAsync(x => x.Id == request.CurrencyId,
            cancellationToken: cancellationToken);
        
        if (currency == default)
        {
            throw new NotFoundException(new ErrorModel
            {
                Message = "Currency does not exist"
            });
        }

        var created = await _rateRepository.InsertAsync(_mapper.Map<Rate>(request), cancellationToken);
        
        await _rateRepository.SaveChangesAsync(cancellationToken);
            
        var result =
            await _rateRepository.GetOneAsync(x => x.Id == created.Id, Included, cancellationToken: cancellationToken);

        return _mapper.Map<RateDetailResponse>(result);
    }
}