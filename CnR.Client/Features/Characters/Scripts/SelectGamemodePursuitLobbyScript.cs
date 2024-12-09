using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Common;
using CnR.Client.Features.Games.Abstractions;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Shared.Dtos;
using CnR.Shared.Uis;

namespace CnR.Client.Features.Characters.Scripts;

public sealed class SelectGamemodePursuitLobbyScript(IUi ui, IEffectfulMessenger messenger, IGame game) : Script
{
    public override Task StartAsync(CancellationToken ct)
    {
        ui.OnMount(Route.GamemodeSelection, OnUiMount);
        return Task.CompletedTask;
    }

    private Action OnUiMount()
    {
        return Merge(
            ui.On("gamemode-selection.pursuit.getLobbies", OnUiGetLobbiesAsync),
            ui.On<object>(
                "gamemode-selection.pursuit.getParticipants",
                (ctx, id) =>
                {
                    if (id is not double idDouble)
                    {
                        return Task.CompletedTask;
                    }
                    return OnUiGetParticipantsAsync(ctx, (long)idDouble);
                }
            ),
            ui.On<object>(
                "gamemode-selection.pursuit.joinLobby",
                (ctx, id) =>
                {
                    if (id is not double idDouble)
                    {
                        return;
                    }
                    OnUiJoinLobby(ctx, (long)idDouble);
                }
            ),
            messenger.On<GamemodeSelectionPursuitPlayerJoinedDto>(
                "gamemode-selection.pursuit.playerJoined",
                (ctx, dto) =>
                {
                    ui.Publish("gamemode-selection.pursuit.playerJoined", [dto]);
                }
            )
        );
    }

    private async Task OnUiGetLobbiesAsync(IMessagingContext ctx)
    {
        var sent = await messenger
            .SendAsync<List<GamemodeSelectionPursuitLobbyDto>>("gamemode-selection.pursuit.getLobbies")
            .ConfigureAwait(false);
        if (sent.TryGetError(out _, out var success))
        {
            return;
        }
        ctx.Respond([success]);
    }

    private async Task OnUiGetParticipantsAsync(IMessagingContext ctx, long lobbyId)
    {
        var sent = await messenger
            .SendAsync<string[]>("gamemode-selection.pursuit.getParticipants", [lobbyId])
            .ConfigureAwait(false);
        if (sent.TryGetError(out _, out var success))
        {
            return;
        }
        ctx.Respond([lobbyId, success]);
    }

    private void OnUiJoinLobby(IMessagingContext _, long lobbyId)
    {
        messenger.Publish("gamemode-selection.pursuit.joinLobby", [lobbyId]);
    }
}
