using CnR.Server.Domain.Models;
using CnR.Server.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnR.Server.Infrastructure.Persistence.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .Property(a => a.Id)
            .HasConversion<EntityGuidConverter<AccountId>>()
            .ValueGeneratedOnAdd();
        builder.HasKey(a => a.Id);
    }
}
