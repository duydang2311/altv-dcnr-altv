using AltV.Community.Messaging.Server.Abstractions;
using AltV.Net.Elements.Entities;

namespace CnR.Server.Features.Messaging.Abstractions;

public interface IEffectfulMessenger : IMessenger
{
    new Task<Effect<object?, GenericError>> SendAsync(IPlayer player, string eventName, object?[]? args = null);
    new Task<Effect<T, GenericError>> SendAsync<T>(IPlayer player, string eventName, object?[]? args = null);
}
