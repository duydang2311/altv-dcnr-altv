namespace CnR.Server.Features.Lobbies.Abstractions;

public interface ILobbyFactory
{
    ILobby CreateLobby(ILobbyOptions lobbyOptions);
}
