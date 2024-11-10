using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class UiMessagingContextFactory : IUiMessagingContextFactory
{
    public IMessagingContext CreateMessagingContext(IUi ui, string eventName)
    {
        return new UiMessagingContext(ui, eventName);
    }
}
