using CnR.Server.Domain.Models;
using CnR.Server.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CnR.Server.Infrastructure.Persistence.Configurations;

public sealed class CharacterConfiguration : IEntityTypeConfiguration<CharacterModel>
{
    public void Configure(EntityTypeBuilder<CharacterModel> builder)
    {
        builder.Property(a => a.Id).HasConversion<EntityGuidConverter<CharacterId>>().ValueGeneratedOnAdd();
        builder.HasKey(a => a.Id);
        builder.HasOne(a => a.Account).WithMany().HasForeignKey(a => a.AccountId);
    }
}
