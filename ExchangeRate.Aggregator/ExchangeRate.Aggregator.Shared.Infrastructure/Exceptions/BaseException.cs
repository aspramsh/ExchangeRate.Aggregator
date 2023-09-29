namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;

public abstract class BaseException : Exception
{
    public BaseException()
    {
    }

    public BaseException(string message)
        : base(message)
    {
    }
}