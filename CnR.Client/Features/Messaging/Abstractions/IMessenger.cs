using AltV.Net.Elements.Entities;

namespace CnR.Client.Features.Messaging.Abstractions;

public interface IMessenger
{
    void Publish(string eventName, object?[]? args = null);
    Task<Effect<object?, GenericError>> SendAsync(string eventName, object?[]? args = null);
    Task<Effect<T, GenericError>> SendAsync<T>(string eventName, object?[]? args = null);
    bool Answer(string eventName, object? answer);

    Action On(string eventName, Action<IMessagingContext> handler);
    Action On(string eventName, Func<IMessagingContext, Task> handler);
    Action On<T1>(string eventName, Action<IMessagingContext, T1> handler);
    Action On<T1>(string eventName, Func<IMessagingContext, T1, Task> handler);
    Action On<T1, T2>(string eventName, Action<IMessagingContext, T1, T2> handler);
    Action On<T1, T2>(string eventName, Func<IMessagingContext, T1, T2, Task> handler);
    Action On<T1, T2, T3>(string eventName, Action<IMessagingContext, T1, T2, T3> handler);
    Action On<T1, T2, T3>(string eventName, Func<IMessagingContext, T1, T2, T3, Task> handler);
    Action On<T1, T2, T3, T4>(string eventName, Action<IMessagingContext, T1, T2, T3, T4> handler);
    Action On<T1, T2, T3, T4>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, Task> handler);
    Action On<T1, T2, T3, T4, T5>(string eventName, Action<IMessagingContext, T1, T2, T3, T4, T5> handler);
    Action On<T1, T2, T3, T4, T5>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, T5, Task> handler);
    Action On<T1, T2, T3, T4, T5, T6>(string eventName, Action<IMessagingContext, T1, T2, T3, T4, T5, T6> handler);
    Action On<T1, T2, T3, T4, T5, T6>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, T5, T6, Task> handler);
    Action On<T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Action<IMessagingContext, T1, T2, T3, T4, T5, T6, T7> handler
    );
    Action On<T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Func<IMessagingContext, T1, T2, T3, T4, T5, T6, T7, Task> handler
    );
}
