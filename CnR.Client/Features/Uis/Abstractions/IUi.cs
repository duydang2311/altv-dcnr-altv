using System.Numerics;
using AltV.Community.Messaging.Client.Abstractions;
using AltV.Net.Client.Elements.Interfaces;
using CnR.Shared.Uis;
using OneOf.Types;

namespace CnR.Client.Features.Uis.Abstractions;

public interface IUi : IBaseObject
{
    IntPtr WebViewNativePointer { get; }
    bool Focused { get; set; }
    bool Overlay { get; }
    bool Visible { get; set; }
    Vector2 Position { get; }
    Vector2 Size { get; set; }
    string Url { get; set; }

    void Initialize();
    void ToggleFocus(bool toggle);
    void Publish(string eventName, object?[]? args = null);
    void Publish(string eventName, long messageId, object?[]? args = null);
    Task<Effect<T, GenericError>> SendAsync<T>(string eventName, object?[]? args = null);
    Task<Effect<None, GenericError>> MountAsync(Route route, object? props = null);
    Task<Effect<None, GenericError>> UnmountAsync(Route route, object? props = null);
    Task<Effect<None, GenericError>> MountAsync(string route, object? props = null);
    Task<Effect<None, GenericError>> UnmountAsync(string route, object? props = null);
    Action OnMount(Route route, Action handler);
    Action OnMount(Route route, Func<Action> handler);
    Action OnMount(Route route, Func<Task> handler);
    Action OnUnmount(Route route, Action handler);
    Action OnUnmount(Route route, Func<Task> handler);

    Action On(string eventName, Action handler);
    Action On(string eventName, Action<IMessagingContext> handler);
    Action On(string eventName, Func<Task> handler);
    Action On(string eventName, Func<IMessagingContext, Task> handler);

    Action On<T1>(string eventName, Action<T1> handler);
    Action On<T1>(string eventName, Func<T1, Task> handler);
    Action On<T1>(string eventName, Action<IMessagingContext, T1> handler);
    Action On<T1>(string eventName, Func<IMessagingContext, T1, Task> handler);

    Action On<T1, T2>(string eventName, Action<T1, T2> handler);
    Action On<T1, T2>(string eventName, Func<T1, T2, Task> handler);
    Action On<T1, T2>(string eventName, Action<IMessagingContext, T1, T2> handler);
    Action On<T1, T2>(string eventName, Func<IMessagingContext, T1, T2, Task> handler);

    Action On<T1, T2, T3>(string eventName, Action<T1, T2, T3> handler);
    Action On<T1, T2, T3>(string eventName, Func<T1, T2, T3, Task> handler);
    Action On<T1, T2, T3>(string eventName, Action<IMessagingContext, T1, T2, T3> handler);
    Action On<T1, T2, T3>(string eventName, Func<IMessagingContext, T1, T2, T3, Task> handler);

    Action On<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> handler);
    Action On<T1, T2, T3, T4>(string eventName, Func<T1, T2, T3, T4, Task> handler);
    Action On<T1, T2, T3, T4>(string eventName, Action<IMessagingContext, T1, T2, T3, T4> handler);
    Action On<T1, T2, T3, T4>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, Task> handler);

    Action On<T1, T2, T3, T4, T5>(string eventName, Action<T1, T2, T3, T4, T5> handler);
    Action On<T1, T2, T3, T4, T5>(string eventName, Func<T1, T2, T3, T4, T5, Task> handler);
    Action On<T1, T2, T3, T4, T5>(string eventName, Action<IMessagingContext, T1, T2, T3, T4, T5> handler);
    Action On<T1, T2, T3, T4, T5>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, T5, Task> handler);

    Action On<T1, T2, T3, T4, T5, T6>(string eventName, Action<T1, T2, T3, T4, T5, T6> handler);
    Action On<T1, T2, T3, T4, T5, T6>(string eventName, Func<T1, T2, T3, T4, T5, T6, Task> handler);
    Action On<T1, T2, T3, T4, T5, T6>(string eventName, Action<IMessagingContext, T1, T2, T3, T4, T5, T6> handler);
    Action On<T1, T2, T3, T4, T5, T6>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, T5, T6, Task> handler);

    Action On<T1, T2, T3, T4, T5, T6, T7>(string eventName, Action<T1, T2, T3, T4, T5, T6, T7> handler);
    Action On<T1, T2, T3, T4, T5, T6, T7>(string eventName, Func<T1, T2, T3, T4, T5, T6, T7, Task> handler);
    Action On<T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Action<IMessagingContext, T1, T2, T3, T4, T5, T6, T7> handler
    );
    Action On<T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Func<IMessagingContext, T1, T2, T3, T4, T5, T6, T7, Task> handler
    );
}
