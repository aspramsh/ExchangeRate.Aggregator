using ExchangeRate.Aggregator.Modules.Parsers.Application.Services;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.Services;

public class RateContext : IRateContext
{
    private readonly IServiceProvider _serviceProvider;

    public RateContext(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IRateService GetConcreteBankService(string name)
    {
        var services = _serviceProvider.GetServices<IRateService>();

        return name switch
        {
            BankARateService.Name => services.First(o => o.GetType() == typeof(BankARateService)),
            _ => throw new BadRequestException(new ErrorModel {Message = "Invalid bank"})
        };
    }
}