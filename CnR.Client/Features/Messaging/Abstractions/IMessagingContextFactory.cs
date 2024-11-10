namespace CnR.Client.Features.Messaging.Abstractions;

public interface IMessagingContextFactory
{
    IMessagingContext CreateMessagingContext(string eventName);
}
