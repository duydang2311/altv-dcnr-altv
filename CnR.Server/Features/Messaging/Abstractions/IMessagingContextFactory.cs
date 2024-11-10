using AltV.Net.Elements.Entities;

namespace CnR.Server.Features.Messaging.Abstractions;

public interface IMessagingContextFactory
{
    IMessagingContext<TPlayer> CreateMessagingContext<TPlayer>(TPlayer player, string eventName)
        where TPlayer : IPlayer;
}
