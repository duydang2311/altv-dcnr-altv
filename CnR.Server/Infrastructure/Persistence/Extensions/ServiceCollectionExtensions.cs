using CnR.Server.Infrastructure.Persistence;
using CnR.Server.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection serviceCollection,
        DbOptions options
    )
    {
        Console.WriteLine("Add persistence: " + options);
        serviceCollection.AddPooledDbContextFactory<Db>(b =>
            b.UseNpgsql(
                    options.ConnectionString,
                    b => b.MigrationsAssembly(options.MigrationAssembly)
                )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
        serviceCollection.AddSingleton<IDbFactory, DbFactory>();
        return serviceCollection;
    }
}
