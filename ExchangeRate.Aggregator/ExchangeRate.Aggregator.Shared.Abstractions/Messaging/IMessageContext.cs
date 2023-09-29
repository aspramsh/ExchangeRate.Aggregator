using ExchangeRate.Aggregator.Shared.Abstractions.Contexts;

namespace ExchangeRate.Aggregator.Shared.Abstractions.Messaging;

public interface IMessageContext
{
    public Guid MessageId { get; }
    
    public IContext Context { get; }
}