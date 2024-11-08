namespace CnR.Server.Domain.Models;

public sealed record DiscordAccount
{
    public AccountId AccountId { get; init; }
    public Account Account { get; init; } = null!;
    public string DiscordId { get; init; } = null!;
    public string DiscordUsername { get; init; } = null!;
    public string? DiscordAvatar { get; init; }
}
