namespace ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;

public static class Constants
{
    public const string TenantHeader = "Tenant";

    public const string ConnectionStringDefaultKey = "DefaultConnectionString";

    public const string PostgresDefaultSchemaName = "public";
        
    public const string DefaultMigrationHistoryTableName = "migration_history";
}