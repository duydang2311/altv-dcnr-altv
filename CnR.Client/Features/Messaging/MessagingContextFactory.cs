using CnR.Client.Features.Messaging.Abstractions;

namespace CnR.Client.Features.Messaging;

public sealed class MessagingContextFactory : IMessagingContextFactory
{
    public IMessagingContext CreateMessagingContext(string eventName)
    {
        return new MessagingContext(eventName);
    }
}
