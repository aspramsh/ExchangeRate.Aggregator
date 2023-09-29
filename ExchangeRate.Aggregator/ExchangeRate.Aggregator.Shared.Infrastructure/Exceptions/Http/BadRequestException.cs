using System.Net;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;

public class BadRequestException : BaseHttpException
{
    public BadRequestException()
        : base(HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(
        ErrorModel errorData)
        : base(HttpStatusCode.BadRequest, errorData)
    {
    }
}