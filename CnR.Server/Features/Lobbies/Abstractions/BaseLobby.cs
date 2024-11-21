using System.Collections.Immutable;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Lobbies.Abstractions;

public abstract class BaseLobby(
    ILobbyOptions lobbyOptions,
    ILobbyPlayerAddedEvent lobbyPlayerAddedEvent,
    ILobbyPlayerRemovedEvent lobbyPlayerRemovedEvent
) : ILobby
{
    private ImmutableHashSet<ICharacter> players = [];

    public long Id { get; init; } = lobbyOptions.Id;
    public string Name { get; init; } = lobbyOptions.Name;

    public ILobbyPlayerAddedEvent PlayerAddedEvent => lobbyPlayerAddedEvent;
    public ILobbyPlayerRemovedEvent PlayerRemovedEvent => lobbyPlayerRemovedEvent;

    public IReadOnlyCollection<ICharacter> GetPlayers()
    {
        return players;
    }

    public bool AddPlayer(ICharacter player)
    {
        if (players.Contains(player))
        {
            return false;
        }
        players = players.Add(player);
        PlayerAddedEvent.Invoke(this, player);
        return true;
    }

    public bool RemovePlayer(ICharacter player)
    {
        if (!players.Contains(player))
        {
            return false;
        }
        players = players.Remove(player);
        PlayerRemovedEvent.Invoke(this, player);
        return true;
    }
}
