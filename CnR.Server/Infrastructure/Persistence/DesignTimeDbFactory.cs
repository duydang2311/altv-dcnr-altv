using CnR.Server.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CnR.Server.Infrastructure.Persistence;

public sealed class DesignTimeDbFactory : IDesignTimeDbContextFactory<Db>
{
    public Db CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Local.json", optional: true)
            .Build();

        var options = configuration.GetSection(DbOptions.Section).Get<DbOptions>()!;

        var optionsBuilder = new DbContextOptionsBuilder<Db>();
        optionsBuilder.UseNpgsql(options.ConnectionString, x => x.MigrationsAssembly(options.MigrationAssembly));
        return new Db(optionsBuilder.Options);
    }
}
