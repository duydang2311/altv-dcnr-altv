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
        var added = players.Add(player);
        if (added != players)
        {
            players = added;
            PlayerAddedEvent.Invoke(this, player);
            return true;
        }
        return false;
    }

    public bool RemovePlayer(ICharacter player)
    {
        var removed = players.Remove(player);
        if (removed != players)
        {
            players = removed;
            PlayerRemovedEvent.Invoke(this, player);
            return true;
        }
        return false;
    }
}
