using AltV.Net;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Interfaces;
using AsyncAwaitBestPractices;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Uis;

public sealed class Ui(
    ICore core,
    IntPtr webViewNativePointer,
    uint id,
    IUiMessagingContextFactory uiMessagingContextFactory
) : WebView(core, webViewNativePointer, id), IUi
{
    private int focusCounter;

    public void Publish(string eventName, object?[]? args = null)
    {
        Emit(eventName, args ?? []);
    }

    public void ToggleFocus(bool toggle)
    {
        if (toggle)
        {
            if (Interlocked.Increment(ref focusCounter) > 0)
            {
                Focus();
            }
        }
        else
        {
            if (Interlocked.Decrement(ref focusCounter) <= 0)
            {
                Unfocus();
            }
        }
    }

    public Action On(string eventName, Action<IMessagingContext> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                () =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName));
                }
            )
        );
    }

    public Action On(string eventName, Func<IMessagingContext, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                () =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName))
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1>(string eventName, Action<IMessagingContext, T1> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1);
                }
            )
        );
    }

    public Action On<T1>(string eventName, Func<IMessagingContext, T1, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2>(string eventName, Action<IMessagingContext, T1, T2> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1, arg2);
                }
            )
        );
    }

    public Action On<T1, T2>(string eventName, Func<IMessagingContext, T1, T2, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1, arg2)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3>(string eventName, Action<IMessagingContext, T1, T2, T3> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1, arg2, arg3);
                }
            )
        );
    }

    public Action On<T1, T2, T3>(string eventName, Func<IMessagingContext, T1, T2, T3, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1, arg2, arg3)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4>(string eventName, Action<IMessagingContext, T1, T2, T3, T4> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1, arg2, arg3, arg4);
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, eventName), arg1, arg2, arg3, arg4)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5>(string eventName, Action<IMessagingContext, T1, T2, T3, T4, T5> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, eventName),
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5
                    );
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, T5, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, eventName),
                            arg1,
                            arg2,
                            arg3,
                            arg4,
                            arg5
                        )
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5, T6>(
        string eventName,
        Action<IMessagingContext, T1, T2, T3, T4, T5, T6> handler
    )
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, eventName),
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5,
                        arg6
                    );
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5, T6>(
        string eventName,
        Func<IMessagingContext, T1, T2, T3, T4, T5, T6, Task> handler
    )
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, eventName),
                            arg1,
                            arg2,
                            arg3,
                            arg4,
                            arg5,
                            arg6
                        )
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Action<IMessagingContext, T1, T2, T3, T4, T5, T6, T7> handler
    )
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, eventName),
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5,
                        arg6,
                        arg7
                    );
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Func<IMessagingContext, T1, T2, T3, T4, T5, T6, T7, Task> handler
    )
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, eventName),
                            arg1,
                            arg2,
                            arg3,
                            arg4,
                            arg5,
                            arg6,
                            arg7
                        )
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On(string eventName, Action handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On(string eventName, Func<Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                () =>
                {
                    handler().SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1>(string eventName, Action<T1> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1>(string eventName, Func<T1, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1) =>
                {
                    handler(arg1).SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2>(string eventName, Action<T1, T2> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1, T2>(string eventName, Func<T1, T2, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2) =>
                {
                    handler(arg1, arg2).SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3>(string eventName, Action<T1, T2, T3> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1, T2, T3>(string eventName, Func<T1, T2, T3, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(arg1, arg2, arg3).SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1, T2, T3, T4>(string eventName, Func<T1, T2, T3, T4, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(arg1, arg2, arg3, arg4).SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5>(string eventName, Action<T1, T2, T3, T4, T5> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1, T2, T3, T4, T5>(string eventName, Func<T1, T2, T3, T4, T5, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(arg1, arg2, arg3, arg4, arg5).SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5, T6>(string eventName, Action<T1, T2, T3, T4, T5, T6> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1, T2, T3, T4, T5, T6>(string eventName, Func<T1, T2, T3, T4, T5, T6, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(arg1, arg2, arg3, arg4, arg5, arg6).SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5, T6, T7>(string eventName, Action<T1, T2, T3, T4, T5, T6, T7> handler)
    {
        return OnInternal(eventName, (this as IWebView).On(eventName, handler));
    }

    public Action On<T1, T2, T3, T4, T5, T6, T7>(string eventName, Func<T1, T2, T3, T4, T5, T6, T7, Task> handler)
    {
        return OnInternal(
            eventName,
            (this as IWebView).On(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    private Action OnInternal(string eventName, Function function)
    {
        return () =>
        {
            this.Off(eventName, function);
        };
    }
}
