using AltV.Net.Elements.Entities;

namespace CnR.Server.Features.Messaging.Abstractions;

public interface IMessagingContext<TPlayer>
    where TPlayer : IPlayer
{
    TPlayer Player { get; }

    void Respond(object? value = null);
}
