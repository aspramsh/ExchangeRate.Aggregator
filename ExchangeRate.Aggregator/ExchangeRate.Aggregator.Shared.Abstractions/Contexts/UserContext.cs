namespace ExchangeRate.Aggregator.Shared.Abstractions.Contexts;

public partial class UserContext
{
    public UserContext()
    {
        AdditionalExceptionInfo = new();
    }

    public Guid RequestUserId { get; set; }

    public Guid? LogUserId { get; set; }

    public string RequestUserIdentityId { get; set; }

    public string RequestUserName { get; set; }
    
    public string AuthToken { get; set; }

    public string RequestUserIP { get; set; }

    public string IdempotencyKey { get; set; }

    public List<Tuple<string, string, string, string, string>> AdditionalExceptionInfo { get; set; }
}