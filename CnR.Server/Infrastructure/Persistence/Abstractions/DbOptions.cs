using System.ComponentModel.DataAnnotations;

namespace CnR.Server.Infrastructure.Persistence.Abstractions;

public sealed record DbOptions
{
    public const string Section = "Db";

    [Required]
    public required string ConnectionString { get; init; }

    [Required]
    public required string MigrationAssembly { get; init; }
}
