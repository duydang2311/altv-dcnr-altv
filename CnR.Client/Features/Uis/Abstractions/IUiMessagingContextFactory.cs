using AltV.Community.Messaging.Client.Abstractions;

namespace CnR.Client.Features.Uis.Abstractions;

public interface IUiMessagingContextFactory
{
    IMessagingContext CreateMessagingContext(IUi ui, string eventName);
}
