namespace CnR.Server.Infrastructure.Persistence.Abstractions;

public interface IDbFactory
{
    Db CreateDb();
    Task<Db> CreateDbAsync(CancellationToken cancellationToken = default);
}
