using AltV.Community.MValueAdapters.Generators;
using AltV.Community.MValueAdapters.Generators.Abstractions;

namespace CnR.Shared.Dtos;

[MValueAdapter(NamingConvention = NamingConvention.CamelCase)]
public sealed record SignInDiscordRequestDto
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Avatar { get; set; }
    public string? Banner { get; set; }
}
