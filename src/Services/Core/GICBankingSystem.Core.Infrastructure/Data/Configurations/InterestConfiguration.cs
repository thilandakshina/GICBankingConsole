using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Infrastructure.Data.Configurations;

public class InterestConfiguration : IEntityTypeConfiguration<InterestEntity>
{
    public void Configure(EntityTypeBuilder<InterestEntity> builder)
    {

        builder.HasKey(e => e.RuleId);
        builder.Property(e => e.RuleId).HasMaxLength(10).IsRequired();

        builder.Property(e => e.Rate)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.EffectiveDate)
            .IsRequired();
    }
}
