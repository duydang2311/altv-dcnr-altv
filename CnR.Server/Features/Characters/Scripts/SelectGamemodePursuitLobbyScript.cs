using AltV.Community.Messaging.Server.Abstractions;
using AltV.Net.Elements.Entities;
using CnR.Server.Common;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;
using CnR.Server.Features.Messaging.Abstractions;
using CnR.Shared.Dtos;

namespace CnR.Server.Features.Characters.Scripts;

public sealed class SelectGamemodePursuitLobbyScript(IEffectfulMessenger messenger, ILobbyFactory lobbyFactory) : Script
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        messenger.On<IPlayer>("gamemode-selection.pursuit.getLobbies", OnGetLobbies);
        messenger.On<IPlayer, long>("gamemode-selection.pursuit.getParticipants", OnGetParticipants);
        messenger.On<ICharacter, long>("gamemode-selection.pursuit.joinLobby", OnJoinLobby);
        return Task.CompletedTask;
    }

    private void OnGetLobbies(IMessagingContext<IPlayer> ctx)
    {
        ctx.Respond(
            [
                lobbyFactory
                    .GetLobbies()
                    .Where(a => a is IPursuitLobby)
                    .Cast<IPursuitLobby>()
                    .Select(a => new GamemodeSelectionPursuitLobbyDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        ParticipantsCount = a.GetPlayers().Count,
                        MaxParticipants = 16
                    })
                    .ToList()
            ]
        );
    }

    private void OnGetParticipants(IMessagingContext<IPlayer> ctx, long lobbyId)
    {
        var lobby = lobbyFactory.FindLobby(a => a.Id == lobbyId);
        if (lobby is null || lobby is not IPursuitLobby pursuitLobby)
        {
            return;
        }

        ctx.Respond([pursuitLobby.GetPlayers().Select(a => a.Name).ToArray()]);
    }

    private void OnJoinLobby(IMessagingContext<ICharacter> ctx, long lobbyId)
    {
        if (lobbyFactory.GetLobbies().Any(a => a.GetPlayers().Contains(ctx.Player)))
        {
            return;
        }

        var lobby = lobbyFactory.FindLobby(a => a.Id == lobbyId);
        if (lobby is null || lobby is not IPursuitLobby pursuitLobby)
        {
            return;
        }

        if (lobby.AddPlayer(ctx.Player))
        {
            messenger.Publish(
                ctx.Player,
                "gamemode-selection.pursuit.playerJoined",
                [new GamemodeSelectionPursuitPlayerJoinedDto { LobbyId = lobbyId, Name = ctx.Player.Name }]
            );
        }
    }
}
