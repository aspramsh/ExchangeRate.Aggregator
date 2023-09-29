using MediatR;

namespace ExchangeRate.Aggregator.Modules.Parsers.Application.Handlers.Commands;

public class GetRatesCommand : IRequest<Unit>
{
}