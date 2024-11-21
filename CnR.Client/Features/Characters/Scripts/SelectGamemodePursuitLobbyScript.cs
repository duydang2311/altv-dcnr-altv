using AltV.Community.Messaging.Client.Abstractions;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Data;
using CnR.Client.Common;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Shared.Dtos;
using CnR.Shared.Uis;

namespace CnR.Client.Features.Characters.Scripts;

public sealed class SelectGamemodePursuitLobbyScript(IUi ui, IEffectfulMessenger messenger) : Script
{
    public override Task StartAsync(CancellationToken ct)
    {
        ui.OnMount(Route.GamemodeSelection, OnUiMount);
        return Task.CompletedTask;
    }

    private Action OnUiMount()
    {
        Alt.OnKeyUp += OnKeyUp;
        return Merge(
            ui.On("gamemode-selection.pursuit.getLobbies", OnUiGetLobbiesAsync),
            ui.On<object>("gamemode-selection.pursuit.getParticipants", (ctx, id) =>
            {
                if (id is not double idDouble)
                {
                    return Task.CompletedTask;
                }
                return OnUiGetParticipantsAsync(ctx, (long)idDouble);
            }),
            ui.On<object>("gamemode-selection.pursuit.joinLobby", (ctx, id) =>
            {
                if (id is not double idDouble)
                {
                    return;
                }
                OnUiJoinLobby(ctx, (long)idDouble);
            }),
            () =>
            {
                Alt.OnKeyUp -= OnKeyUp;
            }
        );
    }

    private async Task OnUiGetLobbiesAsync(IMessagingContext ctx)
    {
        var sent = await messenger.SendAsync<List<GamemodeSelectionPursuitLobbyDto>>("gamemode-selection.pursuit.getLobbies");
        if (sent.TryGetError(out var e, out var success))
        {
            return;
        }
        ctx.Respond([success]);
    }

    private async Task OnUiGetParticipantsAsync(IMessagingContext ctx, long lobbyId)
    {
        var sent = await messenger.SendAsync<string[]>("gamemode-selection.pursuit.getParticipants", [lobbyId]);
        if (sent.TryGetError(out var e, out var success))
        {
            return;
        }
        ctx.Respond([lobbyId, success]);
    }

    private void OnUiJoinLobby(IMessagingContext ctx, long lobbyId)
    {
        messenger.Publish("gamemode-selection.pursuit.joinLobby", [lobbyId]);
    }

    private void OnKeyUp(Key key)
    {
        if (key == Key.Escape)
        {
            _ = ui.UnmountAsync(Route.GamemodeSelection);
        }
    }
}
