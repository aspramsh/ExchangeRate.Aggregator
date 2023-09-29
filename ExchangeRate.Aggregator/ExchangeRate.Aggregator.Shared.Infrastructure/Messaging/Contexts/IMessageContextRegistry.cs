using ExchangeRate.Aggregator.Shared.Abstractions.Messaging;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Messaging.Contexts;

public interface IMessageContextRegistry
{
    void Set(IMessage message, IMessageContext context);
}