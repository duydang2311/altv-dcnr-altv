using System.Numerics;
using AltV.Community.Messaging.Client.Abstractions;
using AltV.Net.Client.Elements.Interfaces;

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

    void ToggleFocus(bool toggle);
    void Publish(string eventName, object?[]? args = null);

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
