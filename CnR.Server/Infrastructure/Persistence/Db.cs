using CnR.Server.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CnR.Server.Infrastructure.Persistence;

public sealed class Db(DbContextOptions options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<DiscordAccount> DiscordAccounts => Set<DiscordAccount>();
    public DbSet<Character> Characters => Set<Character>();

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
