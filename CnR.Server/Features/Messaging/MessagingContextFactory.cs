using AltV.Net.Elements.Entities;
using CnR.Server.Features.Messaging.Abstractions;

namespace CnR.Server.Features.Messaging;

public sealed class MessagingContextFactory : IMessagingContextFactory
{
    public IMessagingContext<TPlayer> CreateMessagingContext<TPlayer>(TPlayer player, string eventName)
        where TPlayer : IPlayer
    {
        return new MessagingContext<TPlayer>(player, eventName);
    }
}
