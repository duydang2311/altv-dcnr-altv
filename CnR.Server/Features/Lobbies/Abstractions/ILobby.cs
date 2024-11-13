using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Lobbies.Abstractions;

public interface ILobby
{
    public long Id { get; init; }
    public string Name { get; init; }

    ILobbyPlayerAddedEvent PlayerAddedEvent { get; }
    ILobbyPlayerRemovedEvent PlayerRemovedEvent { get; }

    IReadOnlyCollection<ICharacter> GetPlayers();
    bool AddPlayer(ICharacter player);
    bool RemovePlayer(ICharacter player);
}
