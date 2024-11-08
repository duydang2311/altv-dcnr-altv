namespace CnR.Server.Domain.Models;

public sealed record Character
{
    public AccountId AccountId { get; init; }
    public Account Account { get; init; } = null!;
    public CharacterId Id { get; init; }
}
