using AltV.Net.Elements.Entities;

namespace CnR.Server.Features.Messaging.Abstractions;

public interface IMessenger
{
    void Publish(IPlayer player, string eventName, object?[]? args = null);
    Task<Effect<object?, GenericError>> SendAsync(IPlayer player, string eventName, object?[]? args = null);
    Task<Effect<T, GenericError>> SendAsync<T>(IPlayer player, string eventName, object?[]? args = null);
    bool Answer(IPlayer player, string eventName, object? answer);

    Action On<TPlayer>(string eventName, Action<IMessagingContext<TPlayer>> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer>(string eventName, Func<IMessagingContext<TPlayer>, Task> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1>(string eventName, Action<IMessagingContext<TPlayer>, T1> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1>(string eventName, Func<IMessagingContext<TPlayer>, T1, Task> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2>(string eventName, Action<IMessagingContext<TPlayer>, T1, T2> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2>(string eventName, Func<IMessagingContext<TPlayer>, T1, T2, Task> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3>(string eventName, Action<IMessagingContext<TPlayer>, T1, T2, T3> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3>(string eventName, Func<IMessagingContext<TPlayer>, T1, T2, T3, Task> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4>(string eventName, Action<IMessagingContext<TPlayer>, T1, T2, T3, T4> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4>(string eventName, Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, Task> handler)
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4, T5>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5> handler
    )
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4, T5>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, Task> handler
    )
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4, T5, T6>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6> handler
    )
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4, T5, T6>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6, Task> handler
    )
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6, T7> handler
    )
        where TPlayer : IPlayer;
    Action On<TPlayer, T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6, T7, Task> handler
    )
        where TPlayer : IPlayer;
}
