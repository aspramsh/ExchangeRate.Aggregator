using ExchangeRate.Aggregator.Shared.Infrastructure.Entities;
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
            new Bank
            {
                Id = 1,
                Name = "Bank A",
                Description = "Bank A...",
                ApiSettings = JsonConvert.SerializeObject(new
                {
                    BaseUrl = "https://api.exchangeratesapi.io/v1/",
                    LatestRatesUrl = "latest?access_key=413d96c4d38020d4cbf67e45d5cca487"
                })
            },
            new Bank
            {
                Id = 2,
                Name = "Bank B",
                Description = "Bank B...",
                ApiSettings = JsonConvert.SerializeObject(new
                {
                    BaseUrl = "",
                })
            }
        });
    }
}