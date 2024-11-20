using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Common;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis.Scripts;

public sealed class UiScript(IEffectfulMessenger messenger, IUi ui) : Script
{
    public override Task StartAsync(CancellationToken ct)
    {
        ui.Initialize();
        messenger.On<string, object?>("ui.mount", OnMount);
        messenger.On<string>("ui.unmount", OnUnmount);
        return Task.CompletedTask;
    }

    private async Task OnMount(IMessagingContext ctx, string route, object? prop)
    {
        var sent = await ui.MountAsync(route).ConfigureAwait(false);
        ctx.Respond([sent.TryGetSuccess(out _, out _)]);
    }

    private async Task OnUnmount(IMessagingContext ctx, string route)
    {
        var sent = await ui.UnmountAsync(route).ConfigureAwait(false);
        ctx.Respond([sent.TryGetSuccess(out _, out _)]);
    }
}
