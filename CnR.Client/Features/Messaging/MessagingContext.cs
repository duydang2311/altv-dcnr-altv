using AltV.Net.Client;
using CnR.Client.Features.Messaging.Abstractions;

namespace CnR.Client.Features.Messaging;

public sealed class MessagingContext(string eventName) : IMessagingContext
{
    private int respondStarted;

    public void Respond(object? value = null)
    {
        if (Interlocked.CompareExchange(ref respondStarted, 1, 0) == 1)
        {
            return;
        }
        Alt.EmitServer(eventName, value);
    }
}
