using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class UiMessagingContextFactory : IUiMessagingContextFactory
{
    public IMessagingContext CreateMessagingContext(IUi ui, long messageId, string eventName)
    {
        return new UiMessagingContext(ui, messageId, eventName);
    }
}
