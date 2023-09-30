using ExchangeRate.Aggregator.Modules.Parsers.Application.Services;
using ExchangeRate.Aggregator.Shared.Abstractions.Repositories;
using MediatR;

namespace ExchangeRate.Aggregator.Modules.Parsers.Application.Handlers.Commands;

public class GetRatesHandler : IRequestHandler<GetRatesCommand, Unit>
{
    private readonly IBankRepository _bankRepository;
    private readonly IRateContext _rateContext;
    
    public GetRatesHandler(
        IBankRepository bankRepository,
        IRateContext context)
    {
        _bankRepository = bankRepository;
        _rateContext = context;
    }
    
    public async Task<Unit> Handle(GetRatesCommand request, CancellationToken cancellationToken)
    {
        var activeBanks = await _bankRepository.FindByAsync(b => true, cancellationToken: cancellationToken);

        foreach (var bank in activeBanks)
        {
            var service = _rateContext.GetConcreteBankService(bank.Name);
            
            // TODO: get for all base currencies, the API is not free
            await service.GetLatestRatesAsync(bank, cancellationToken: cancellationToken);
        }

        return Unit.Value;
    }
}