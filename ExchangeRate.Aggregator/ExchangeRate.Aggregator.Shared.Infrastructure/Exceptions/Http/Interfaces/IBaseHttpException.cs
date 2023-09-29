namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http.Interfaces;

public interface IBaseHttpException<out T>
    where T : Enum
{
    T StatusCode { get; }
}