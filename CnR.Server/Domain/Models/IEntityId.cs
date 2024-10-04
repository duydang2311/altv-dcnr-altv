namespace CnR.Server.Domain.Models;

public interface IEntityId<T>
{
    T Value { get; init; }
}
