using AltV.Community.Messaging.Server.Abstractions;
using AltV.Net.Elements.Entities;
using CnR.Server.Common;
using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;
using CnR.Server.Features.Messaging.Abstractions;
using CnR.Shared.Dtos;

namespace CnR.Server.Features.Characters.Scripts;

public sealed class SelectGamemodeScript(IEffectfulMessenger messenger, ILobbyFactory lobbyFactory) : Script
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        messenger.On<IPlayer>("gamemode-selection.pursuit.getLobbies", OnGetPursuitLobbies);
        messenger.On<IPlayer, long>("gamemode-selection.pursuit.getParticipants", OnGetPursuitParticipants);
        return Task.CompletedTask;
    }

    private void OnGetPursuitLobbies(IMessagingContext<IPlayer> ctx)
    {
        ctx.Respond([
            lobbyFactory
                .GetLobbies()
                .Where(a => a is IPursuitLobby)
                .Cast<IPursuitLobby>()
                .Select(a => new GamemodeSelectionPursuitLobbyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                })
                .ToList()
        ]);
    }

    private void OnGetPursuitParticipants(IMessagingContext<IPlayer> ctx, long lobbyId)
    {
        var lobby = lobbyFactory.FindLobby(a => a.Id == lobbyId);
        if (lobby is null || lobby is not IPursuitLobby pursuitLobby)
        {
            return;
        }

        ctx.Respond(pursuitLobby.GetPlayers().Select(a => a.Name).ToArray());
    }
}
