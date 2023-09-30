using ExchangeRate.Aggregator.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.Property(x => x.Name).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.ApiSettings).HasColumnType("jsonb");

        builder.HasData(new List<Bank>
        {
            new()
            {
                Id = 1,
                Name = "Bank A",
                Description = "Bank A...",
                ApiSettings = JsonConvert.SerializeObject(new
                {
                    LatestRatesUrl = "http://api.exchangeratesapi.io/v1/latest?access_key=413d96c4d38020d4cbf67e45d5cca487"
                })
            },
        });
    }
}