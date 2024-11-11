namespace CnR.Server.Domain.Models;

public sealed record DiscordAccountModel
{
    public AccountId AccountId { get; init; }
    public AccountModel Account { get; init; } = null!;
    public string DiscordId { get; init; } = null!;
    public string DiscordUsername { get; init; } = null!;
    public string? DiscordAvatar { get; init; }
}
