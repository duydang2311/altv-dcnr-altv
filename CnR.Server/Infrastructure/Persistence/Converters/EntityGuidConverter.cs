using CnR.Server.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CnR.Server.Infrastructure.Persistence.Converters;

public sealed class EntityGuidConverter<T> : ValueConverter<T, Guid>
    where T : IEntityId<Guid>, new()
{
    public EntityGuidConverter()
        : base(a => a.Value, a => new T() { Value = a }) { }
}
