using System.Net;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;

public class InternalServerException : BaseHttpException
{
    public InternalServerException()
        : base(HttpStatusCode.InternalServerError, new ErrorModel
        {
            Message = "Unexpected error occured",
        })
    {
    }

    public InternalServerException(Exception exception)
        : base(HttpStatusCode.InternalServerError, new ErrorModel(exception.GetaAllMessages()))
    {
    }
}