using AltV.Net.Elements.Entities;
using CnR.Server.Features.Messaging.Abstractions;
using CnR.Server.Features.Uis.Abstractions;
using CnR.Shared.Uis;
using OneOf.Types;

namespace CnR.Server.Features.Uis;

public sealed class Ui(IEffectfulMessenger messenger) : IUi
{
    public void Mount(IPlayer player, Route route, object? prop = null)
    {
        messenger.Publish(player, "ui.mount", [route.Value, prop]);
    }

    public async Task<Effect<None, GenericError>> MountAsync(IPlayer player, Route route, object? prop = null)
    {
        var sent = await messenger.SendAsync(player, "ui.mount", [route.Value, prop]).ConfigureAwait(false);
        if (sent.TryGetError(out var e, out _))
        {
            return Effect.Fail(e);
        }
        return Effect.Succeed(new None());
    }

    public void Unmount(IPlayer player, Route route)
    {
        messenger.Publish(player, "ui.unmount", [route.Value]);
    }

    public async Task<Effect<None, GenericError>> UnmountAsync(IPlayer player, Route route)
    {
        var sent = await messenger.SendAsync(player, "ui.unmount", [route.Value]).ConfigureAwait(false);
        if (sent.TryGetError(out var e, out _))
        {
            return Effect.Fail(e);
        }
        return Effect.Succeed(new None());
    }
}
