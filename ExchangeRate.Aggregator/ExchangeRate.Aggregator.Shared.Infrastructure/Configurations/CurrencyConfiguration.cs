using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.Property(x => x.Name).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Code).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasData(new List<Currency>
        {
            new()
            {
                Id = 1,
                Code = "USD",
                Name = "Unites States Dollar",
                Description = "Unites States Dollar"
            },
            new()
            {
                Id = 2,
                Code = "EUR",
                Name = "Euro",
                Description = "Euro"
            }
        });
    }
}