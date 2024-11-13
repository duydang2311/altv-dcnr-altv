namespace CnR.Server.Features.Lobbies.Abstractions;

public abstract record BaseLobbyOptions : ILobbyOptions
{
    public required long Id { get; init; }
    public required string Name { get; init; }
}
