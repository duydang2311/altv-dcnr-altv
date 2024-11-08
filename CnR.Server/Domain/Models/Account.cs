namespace CnR.Server.Domain.Models;

public sealed record Account
{
    public AccountId Id { get; init; }
    public DiscordAccount? Discord { get; init; }
}
