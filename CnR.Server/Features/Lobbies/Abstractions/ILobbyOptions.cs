namespace CnR.Server.Features.Lobbies.Abstractions;

public interface ILobbyOptions
{
    long Id { get; init; }
    string Name { get; init; }
}
