namespace CnR.Server.Features.Lobbies.Abstractions;

public interface ILobbyFactory
{
    ILobby CreateLobby(ILobbyOptions lobbyOptions);
    ILobby? FindLobby(Predicate<ILobby> match);
    IEnumerable<ILobby> GetLobbies();
}
