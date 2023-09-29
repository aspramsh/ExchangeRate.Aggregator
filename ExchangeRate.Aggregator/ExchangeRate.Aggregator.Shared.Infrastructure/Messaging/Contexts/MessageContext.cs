using ExchangeRate.Aggregator.Shared.Abstractions.Contexts;
using ExchangeRate.Aggregator.Shared.Abstractions.Messaging;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Messaging.Contexts;

public class MessageContext : IMessageContext
{
    public MessageContext(Guid messageId, IContext context)
    {
        MessageId = messageId;
        Context = context;
    }
    
    public Guid MessageId { get; }
    
    public IContext Context { get; }
}