using System.Net;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http.Interfaces;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;

public abstract class BaseHttpException : BaseException, IBaseHttpException<HttpStatusCode>
{
    public BaseHttpException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public BaseHttpException(
        HttpStatusCode statusCode,
        ErrorModel errorData)
        : base(errorData.Message)
    {
        StatusCode = statusCode;
        ErrorObject = errorData;
    }

    public HttpStatusCode StatusCode { get;  }

    public ErrorModel ErrorObject { get; }
}