using AltV.Community.Messaging.Abstractions;
using AltV.Community.Messaging.Client;
using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Shared.Errors;

namespace CnR.Client.Features.Messaging;

public sealed class EffectfulMessenger(
    IMessagingContextFactory messagingContextFactory,
    IMessageIdProvider messageIdProvider
) : Messenger(messagingContextFactory, messageIdProvider), IEffectfulMessenger
{
    public new Task<Effect<object?, GenericError>> SendAsync(string eventName, object?[]? args = null)
    {
        return SendAsync<object?>(eventName, args);
    }

    public new async Task<Effect<T, GenericError>> SendAsync<T>(string eventName, object?[]? args = null)
    {
        try
        {
            var ret = await base.SendAsync<T>(eventName, args).ConfigureAwait(false);
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
