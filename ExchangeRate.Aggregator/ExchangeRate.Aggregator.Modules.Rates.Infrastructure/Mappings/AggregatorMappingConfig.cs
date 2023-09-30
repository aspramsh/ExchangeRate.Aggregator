using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.CreateRate;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.ResponseModels;
using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using Mapster;

namespace ExchangeRate.Aggregator.Modules.Rates.Infrastructure.Mappings;

public class AggregatorMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Rate, RateDetailResponse>();
        
        config.NewConfig<CreateRateCommand, Rate>()
            .Map(dest => dest.DateTime, src => DateTime.UtcNow);;

        config.NewConfig<Currency, CurrencyResponse>();
        
        config.NewConfig<Bank, BankResponse>();
    }
}