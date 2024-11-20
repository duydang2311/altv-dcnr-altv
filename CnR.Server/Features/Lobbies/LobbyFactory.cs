using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CnR.Server.Features.Lobbies;

public sealed class LobbyFactory(
    IServiceProvider serviceProvider,
    ILobbyCreatedEvent lobbyCreatedEvent,
    PursuitLobbyStatusChangedEvent pursuitLobbyStatusChangedEvent
) : ILobbyFactory
{
    private readonly List<ILobby> lobbies = [];

    public ILobby CreateLobby(ILobbyOptions lobbyOptions)
    {
        ILobby lobby = lobbyOptions switch
        {
            PursuitLobbyOptions createPursuitLobbyOptions
                => new PursuitLobby(
                    createPursuitLobbyOptions,
                    serviceProvider.GetRequiredService<ILobbyPlayerAddedEvent>(),
                    serviceProvider.GetRequiredService<ILobbyPlayerRemovedEvent>(),
                    pursuitLobbyStatusChangedEvent
                ),
            _ => throw new InvalidOperationException(),
        };
        lobbies.Add(lobby);
        lobbyCreatedEvent.Invoke(lobby);
        return lobby;
    }

    public ILobby? FindLobby(Predicate<ILobby> match)
    {
        return lobbies.Find(match);
    }

    public IEnumerable<ILobby> GetLobbies()
    {
        return lobbies;
    }
}
