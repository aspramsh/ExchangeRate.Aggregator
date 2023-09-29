using System.Net;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;

public class NotFoundException : BaseHttpException
{
    public NotFoundException()
        : base(HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string messageKey, string message, string field = default)
        : base(HttpStatusCode.NotFound, new ErrorModel(messageKey, message, field))
    {
    }

    public NotFoundException(ErrorModel errorModel)
        : base(HttpStatusCode.NotFound, errorModel)
    {
    }
}