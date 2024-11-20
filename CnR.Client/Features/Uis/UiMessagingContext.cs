using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class UiMessagingContext(IUi ui, long messageId, string eventName) : IMessagingContext
{
    private int responded;

    public void Respond(object?[]? args = null)
    {
        if (Interlocked.CompareExchange(ref responded, 1, 0) == 1)
        {
            return;
        }

        ui.Publish(eventName, messageId, args);
    }
}
