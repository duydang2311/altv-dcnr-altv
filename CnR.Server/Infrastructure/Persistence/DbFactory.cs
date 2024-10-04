using CnR.Server.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CnR.Server.Infrastructure.Persistence;

public sealed class DbFactory(IDbContextFactory<Db> factory) : IDbFactory
{
    public Db CreateDb() => factory.CreateDbContext();

    public Task<Db> CreateDbAsync(CancellationToken cancellationToken = default) =>
        factory.CreateDbContextAsync(cancellationToken);
}
