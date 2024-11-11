namespace CnR.Server.Domain.Models;

public sealed record AccountModel
{
    public AccountId Id { get; init; }
    public DiscordAccountModel? Discord { get; init; }
}
