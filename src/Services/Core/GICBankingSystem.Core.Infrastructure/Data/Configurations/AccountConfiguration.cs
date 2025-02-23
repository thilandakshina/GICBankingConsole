using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.HasKey(e => e.AccountNo);

        builder.Property(e => e.AccountNo)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.AccountNo)
            .IsUnique();

        builder.Property(e => e.CreatedDate)
            .IsRequired();
    }
}
