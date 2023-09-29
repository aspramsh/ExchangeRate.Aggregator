namespace ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;

public class MigrationOptions
{
    public string AssemblyName { get; set; }
    
    public string SchemaName { get; set; }
    
    public string TableName { get; set; }
}