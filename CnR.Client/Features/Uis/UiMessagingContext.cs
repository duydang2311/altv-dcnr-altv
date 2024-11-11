using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class UiMessagingContext(IUi ui, string eventName) : IMessagingContext
{
    private int responseStarted;

    public void Respond(object? value = null)
    {
        if (Interlocked.CompareExchange(ref responseStarted, 1, 0) == 1)
        {
            return;
        }
        ui.Publish(eventName, value is null ? [] : [value]);
    }
}
