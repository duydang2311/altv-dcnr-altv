using AltV.Net.Elements.Entities;
using CnR.Server.Features.Messaging.Abstractions;

namespace CnR.Server.Features.Messaging;

public sealed class MessagingContext<TPlayer>(TPlayer player, string eventName) : IMessagingContext<TPlayer>
    where TPlayer : IPlayer
{
    private int respondStarted;

    public TPlayer Player => player;

    public void Respond(object? value = null)
    {
        if (Interlocked.CompareExchange(ref respondStarted, 1, 0) == 1)
        {
            return;
        }
        player.Emit(eventName, value);
    }
}
