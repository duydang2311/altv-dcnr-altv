using CnR.Server.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CnR.Server.Infrastructure.Persistence;

public sealed class Db(DbContextOptions options) : DbContext(options)
{
    public DbSet<AccountModel> Accounts => Set<AccountModel>();
    public DbSet<DiscordAccountModel> DiscordAccounts => Set<DiscordAccountModel>();
    public DbSet<CharacterModel> Characters => Set<CharacterModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Db).Assembly);
    }
}
