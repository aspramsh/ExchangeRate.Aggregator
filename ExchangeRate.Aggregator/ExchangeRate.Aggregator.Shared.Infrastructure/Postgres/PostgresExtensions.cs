using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.NameTranslation;
using Pluralize.NET.Core;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Postgres;

public static class PostgresExtensions
{
    public static EntityFrameworkOptions GetPostgresOptions(this IServiceCollection services, string sectionName)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            throw new ArgumentException("Invalid name", nameof(sectionName));
        }

        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>()
                            ?? throw new InvalidOperationException();
        services.Configure<EntityFrameworkOptions>(configuration.GetSection(sectionName));
        var postgresOptions = configuration.GetOptions<EntityFrameworkOptions>(sectionName);

        return postgresOptions;
    }

    /// <summary>
    ///  Registers the Postgres Context as a service in the Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <typeparam name="T">The type of context to be registered.</typeparam>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
    /// <param name="connectionKey">The Connection key.</param>
    /// <param name="certificate">A byte array containing data from an X.509 certificate.</param>
    /// <param name="commandTimeout">The time to wait for the command to execute.</param>
    /// <param name="serviceLifetime">The lifetime with which to register the PostgresContext service in the container.</param>
    /// <param name="migrationOptions">Migration options.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddPostgresContext<T>(
        this IServiceCollection services,
        string connectionKey = Constants.ConnectionStringDefaultKey,
        byte[] certificate = null,
        int commandTimeout = 1,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
        MigrationOptions migrationOptions = default)
        where T : DbContext
    {
        var postgresOptions = services.GetPostgresOptions("postgres");

        return services.AddPostgresContext<T>(
            postgresOptions,
            connectionKey,
            certificate,
            commandTimeout,
            serviceLifetime,
            migrationOptions);
    }

    /// <summary>
    /// Registers the Postgres Context with a given postgres options
    /// as a service in the Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// This method is used whenever some of predifined options has to be changed.
    /// </summary>
    /// <typeparam name="T">The type of context to be registered.</typeparam>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
    /// <param name="postgresOptions">The Postgres options used for configuring Postgres Context.</param>
    /// <param name="connectionKey">The Connection key.</param>
    /// <param name="certificate">A byte array containing data from an X.509 certificate.</param>
    /// <param name="commandTimeout">The time to wait for the command to execute.</param>
    /// <param name="serviceLifetime">The lifetime with which to register the PostgresContext service in the container.</param>
    /// <param name="migrationOptions">Migration options.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddPostgresContext<T>(
        this IServiceCollection services,
        EntityFrameworkOptions postgresOptions,
        string connectionKey = Constants.TenantHeader,
        byte[] certificate = null,
        int commandTimeout = 1,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
        MigrationOptions migrationOptions = null)
        where T : DbContext
    {
        if (!postgresOptions.Enabled)
        {
            return services;
        }

        var connectionString = postgresOptions.Connections[connectionKey] ??
                               throw new ArgumentException(connectionKey);

        return services.AddDbContext<T>(
            (_, options) =>
            {
                options.UseNpgsql(
                    connectionString,
                    serverDbContextOptionsBuilder =>
                    {
                        ConfigureNpgsqlDbContextOptionsBuilder<T>(
                            serverDbContextOptionsBuilder,
                            postgresOptions,
                            certificate,
                            commandTimeout,
                            migrationOptions?.AssemblyName,
                            migrationOptions?.SchemaName,
                            migrationOptions?.TableName);
                    });

                ConfigureDbContextOptionsBuilder(options, postgresOptions);
            },
            serviceLifetime);
    }

    public static void PostgresModelCreating(
        this ModelBuilder builder,
        string schemaName = Constants.PostgresDefaultSchemaName)
    {
        var mapper = new NpgsqlSnakeCaseNameTranslator();

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            // modify column names
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(mapper.TranslateMemberName(
                    property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName(), entity.GetSchema()))));
            }

            // Set schema name
            if (schemaName != Constants.PostgresDefaultSchemaName)
            {
                entity.SetSchema(schemaName);
            }

            // modify table name
            entity.SetTableName(mapper.TranslateMemberName(new Pluralizer().Singularize(entity.GetTableName())));

            // modify keys names
            foreach (var key in entity.GetKeys())
            {
                key.SetName(mapper.TranslateMemberName(key.GetName()));
            }

            // modify foreign keys names
            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(mapper.TranslateMemberName(key.GetConstraintName()));
            }

            // modify indexes names
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(mapper.TranslateMemberName(index.GetDatabaseName()));
            }

            // move asp_net tables into schema 'identity'
            if (entity.GetTableName().StartsWith("asp_net_"))
            {
                entity.SetTableName(entity.GetTableName().Replace("asp_net_", string.Empty));
            }
        }
    }

    private static void ConfigureNpgsqlDbContextOptionsBuilder<T>(
        NpgsqlDbContextOptionsBuilder builder,
        EntityFrameworkOptions postgresOptions,
        byte[] certificate = null,
        int commandTimeout = 1,
        string migrationAssemblyName = null,
        string migrationHistorySchemaName = null,
        string migrationHistoryTableName = null)
        where T : DbContext
    {
        var seconds = (int) TimeSpan.FromMinutes(commandTimeout).TotalSeconds;
        builder.CommandTimeout(seconds);

        if (postgresOptions.EnableRetryOnFailure)
        {
            builder.EnableRetryOnFailure();
        }

        builder.MigrationsAssembly(migrationAssemblyName ?? typeof(T).Assembly.FullName);
        builder.MigrationsHistoryTable(
            migrationHistoryTableName ?? Constants.DefaultMigrationHistoryTableName,
            migrationHistorySchemaName ?? Constants.PostgresDefaultSchemaName);

        if (certificate != null && certificate.Length > 0)
        {
            builder.ProvideClientCertificatesCallback(clientCerts =>
            {
                var cert = new X509Certificate(certificate);
                clientCerts.Add(cert);
            });
        }
    }

    private static void ConfigureDbContextOptionsBuilder(
        DbContextOptionsBuilder dbContextOptionsBuilder,
        EntityFrameworkOptions postgresOptions)
    {
        if (postgresOptions.EnableSensitiveDataLogging)
        {
            dbContextOptionsBuilder.EnableSensitiveDataLogging();
        }

        if (postgresOptions.EnableDetailedErrors)
        {
            dbContextOptionsBuilder.EnableDetailedErrors();
        }

        if (postgresOptions.UseQueryTrackingBehavior)
        {
            dbContextOptionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }

        dbContextOptionsBuilder.EnableServiceProviderCaching(postgresOptions.EnableServiceProviderCaching);
    }

    private static TModel GetOptions<TModel>(this IConfiguration configuration, string section)
        where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);

        return model;
    }
}