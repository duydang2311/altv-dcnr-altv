namespace CnR.Client.Features.Messaging.Abstractions;

public interface IMessagingContext
{
    void Respond(object? value = null);
}
