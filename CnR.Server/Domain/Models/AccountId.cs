namespace CnR.Server.Domain.Models;

public readonly record struct AccountId(Guid Value) : IEntityId<Guid> { }
