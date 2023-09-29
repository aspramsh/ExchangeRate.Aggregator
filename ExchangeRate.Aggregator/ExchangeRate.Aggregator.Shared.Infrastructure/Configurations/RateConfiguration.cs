using ExchangeRate.Aggregator.Shared.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Configurations;

public class RateConfiguration : IEntityTypeConfiguration<Rate>
{
    public void Configure(EntityTypeBuilder<Rate> builder)
    {
        builder.Property(x => x.BaseCurrencyId).IsRequired();
        
        builder.Property(x => x.CurrencyId).IsRequired();
        
        builder.Property(x => x.DateTime).IsRequired();
        
        builder.Property(x => x.BankId).IsRequired();

        builder.HasIndex(x => new { x.DateTime, x.CurrencyId, x.BaseCurrencyId, x.BankId }).IsUnique();

        builder.HasOne(r => r.Currency)
            .WithMany(c => c.Rates)
            .HasForeignKey(r => r.CurrencyId);

        builder.HasOne(r => r.Bank)
            .WithMany(c => c.Rates)
            .HasForeignKey(r => r.BankId);
    }
}