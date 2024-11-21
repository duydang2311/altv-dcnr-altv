using AltV.Net.Client;
using CnR.Client.Common;
using CnR.Client.Features.Games.Abstractions;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Shared.Uis;

namespace CnR.Client.Features.Characters.Scripts;

public sealed class SelectGamemodeScript(IUi ui, IGame game) : Script
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
            () =>
            {
                game.ToggleControls(true);
                game.ToggleCursor(false);
                ui.ToggleFocus(false);
            }
        );
    }
}
