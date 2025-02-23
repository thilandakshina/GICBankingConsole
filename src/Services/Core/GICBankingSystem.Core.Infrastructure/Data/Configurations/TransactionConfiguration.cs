using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.TransactionId).HasMaxLength(15).IsRequired();
        
        builder.Property(e => e.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.Property(e => e.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(e => e.AccountNo)
            .IsRequired();
    }
}
