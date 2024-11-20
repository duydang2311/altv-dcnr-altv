using AltV.Community.Messaging.Client.Abstractions;
using AltV.Net.Client;
using CnR.Client.Common;
using CnR.Client.Features.Games.Abstractions;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Shared.Dtos;
using CnR.Shared.Uis;

namespace CnR.Client.Features.Characters.Scripts;

public sealed class SelectGamemodeScript(IUi ui, IGame game, IEffectfulMessenger messenger) : Script
{
    public override Task StartAsync(CancellationToken ct)
    {
        ui.OnMount(Route.GamemodeSelection, OnUiMount);
        Alt.OnConsoleCommand += (cmd, args) =>
        {
            if (cmd == "select")
            {
                _ = ui.MountAsync(Route.GamemodeSelection);
            }
        };
        return Task.CompletedTask;
    }

    private Action OnUiMount()
    {
        game.ToggleControls(false);
        game.ToggleCursor(true);
        ui.ToggleFocus(true);
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
            () =>
            {
                game.ToggleControls(true);
                game.ToggleCursor(false);
                ui.ToggleFocus(false);
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
}
