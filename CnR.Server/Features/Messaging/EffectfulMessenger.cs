using AltV.Community.Messaging.Abstractions;
using AltV.Community.Messaging.Server;
using AltV.Community.Messaging.Server.Abstractions;
using AltV.Net.Elements.Entities;
using CnR.Server.Features.Messaging.Abstractions;
using CnR.Shared.Errors;

namespace CnR.Server.Features.Messaging;

public sealed class EffectfulMessenger(
    IMessagingContextFactory messagingContextFactory,
    IMessageIdProvider messageIdProvider
) : Messenger(messagingContextFactory, messageIdProvider), IEffectfulMessenger
{
    public new Task<Effect<object?, GenericError>> SendAsync(IPlayer player, string eventName, object?[]? args = null)
    {
        return SendAsync<object?>(player, eventName, args);
    }

    public new async Task<Effect<T, GenericError>> SendAsync<T>(
        IPlayer player,
        string eventName,
        object?[]? args = null
    )
    {
        try
        {
            var ret = await base.SendAsync<T>(player, eventName, args).ConfigureAwait(false);
            return Effect.Succeed(ret);
        }
        catch (TypeMismatchException)
        {
            return Effect.Fail(GenericError.From(new TypeMismatchError()));
        }
        catch (OperationCanceledException)
        {
            return Effect.Fail(GenericError.From(new OperationCanceledError()));
        }
    }
}
