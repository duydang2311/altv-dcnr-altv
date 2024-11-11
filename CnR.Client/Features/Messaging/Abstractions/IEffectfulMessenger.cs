using AltV.Community.Messaging.Client.Abstractions;

namespace CnR.Client.Features.Messaging.Abstractions;

public interface IEffectfulMessenger : IMessenger
{
    new Task<Effect<object?, GenericError>> SendAsync(string eventName, object?[]? args = null);
    new Task<Effect<T, GenericError>> SendAsync<T>(string eventName, object?[]? args = null);
}
