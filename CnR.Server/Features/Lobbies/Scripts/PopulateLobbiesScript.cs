using CnR.Server.Common;
using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;

namespace CnR.Server.Features.Lobbies.Scripts;

public sealed class PopulateLobbiesScript(ILobbyFactory lobbyFactory) : Script
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        lobbyFactory.CreateLobby(new PursuitLobbyOptions() { Id = 1, Name = "Pursuit #1" });
        lobbyFactory.CreateLobby(new PursuitLobbyOptions() { Id = 2, Name = "Pursuit #2" });
        lobbyFactory.CreateLobby(new PursuitLobbyOptions() { Id = 3, Name = "Pursuit #3" });
        lobbyFactory.CreateLobby(new PursuitLobbyOptions() { Id = 4, Name = "Pursuit #4" });
        return Task.CompletedTask;
    }
}
