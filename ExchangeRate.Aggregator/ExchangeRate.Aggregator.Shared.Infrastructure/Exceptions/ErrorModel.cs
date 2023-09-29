namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;

public class ErrorModel
{
    public ErrorModel()
    {
    }

    public ErrorModel(
        string messageKey,
        string message,
        string field = default,
        IEnumerable<ErrorModel> errors = default)
    {
        MessageKey = messageKey;
        Message = message;
        Field = field;
        Errors = errors;
    }

    public ErrorModel(
        IEnumerable<ErrorModel> errors)
    {
        Errors = errors;
    }

    public string Message { get; set; }

    public string MessageKey { get; set; }

    public string Field { get; set; }

    public IEnumerable<ErrorModel> Errors { get; set; }
}