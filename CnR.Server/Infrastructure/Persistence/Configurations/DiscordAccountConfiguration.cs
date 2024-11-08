using CnR.Server.Domain.Models;
using CnR.Server.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnR.Server.Infrastructure.Persistence.Configurations;

public sealed class DiscordAccountConfiguration : IEntityTypeConfiguration<DiscordAccount>
{
    public void Configure(EntityTypeBuilder<DiscordAccount> builder)
    {
        builder
            .Property(a => a.AccountId)
            .HasConversion<EntityGuidConverter<AccountId>>()
            .ValueGeneratedNever();
        builder.HasKey(a => a.AccountId);
        builder.HasIndex(a => a.DiscordId);
        builder.HasOne(a => a.Account).WithOne(a => a.Discord).HasForeignKey<DiscordAccount>(a => a.AccountId);
    }
}
