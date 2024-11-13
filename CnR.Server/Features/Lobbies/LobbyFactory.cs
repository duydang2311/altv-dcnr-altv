using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CnR.Server.Features.Lobbies;

public sealed class LobbyFactory(IServiceProvider serviceProvider, ILobbyCreatedEvent lobbyCreatedEvent) : ILobbyFactory
{
    public ILobby CreateLobby(ILobbyOptions lobbyOptions)
    {
        ILobby lobby = lobbyOptions switch
        {
            PursuitLobbyOptions createPursuitLobbyOptions
                => new PursuitLobby(
                    createPursuitLobbyOptions,
                    serviceProvider.GetRequiredService<ILobbyPlayerAddedEvent>(),
                    serviceProvider.GetRequiredService<ILobbyPlayerRemovedEvent>()
                ),
            _ => throw new InvalidOperationException(),
        };
        lobbyCreatedEvent.Invoke(lobby);
        return lobby;
    }
}
