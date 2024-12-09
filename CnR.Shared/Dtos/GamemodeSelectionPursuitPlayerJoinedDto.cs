using AltV.Community.MValueAdapters.Generators;
using AltV.Community.MValueAdapters.Generators.Abstractions;

namespace CnR.Shared.Dtos;

[MValueAdapter(NamingConvention = NamingConvention.CamelCase)]
public sealed record GamemodeSelectionPursuitPlayerJoinedDto
{
    public long LobbyId { get; set; }
    public string Name { get; set; } = string.Empty;
}
