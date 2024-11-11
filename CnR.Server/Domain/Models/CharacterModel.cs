namespace CnR.Server.Domain.Models;

public sealed record CharacterModel
{
    public AccountId AccountId { get; init; }
    public AccountModel Account { get; init; } = null!;
    public CharacterId Id { get; init; }
}
