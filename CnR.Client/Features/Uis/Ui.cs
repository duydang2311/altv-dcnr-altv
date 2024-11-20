using System.Collections.Concurrent;
using AltV.Community.Messaging.Abstractions;
using AltV.Community.Messaging.Client.Abstractions;
using AltV.Net;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Interfaces;
using AsyncAwaitBestPractices;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Shared.Errors;
using CnR.Shared.Uis;
using OneOf.Types;

namespace CnR.Client.Features.Uis;

public sealed class Ui(
    ICore core,
    IntPtr webViewNativePointer,
    uint id,
    IUiMessagingContextFactory uiMessagingContextFactory,
    IMessageIdProvider messageIdProvider
) : WebView(core, webViewNativePointer, id), IUi
{
    private readonly ConcurrentDictionary<long, StateBag> sendBags = [];
    private readonly ConcurrentDictionary<string, Function> sendHandlers = [];
    private readonly ConcurrentDictionary<string, SimpleStateBag> mountBags = [];
    private readonly ConcurrentDictionary<string, SimpleStateBag> unmountBags = [];
    private readonly ConcurrentDictionary<string, HashSet<Delegate>> onMountHandlers = [];
    private readonly ConcurrentDictionary<string, HashSet<Action>> onUnmountHandlers = [];
    private readonly ConcurrentDictionary<string, HashSet<Action>> onceUnmountHandlers = [];
    private int focusCounter;

    public void Initialize()
    {
        (this as IWebView).On(
            "router.mount",
            (string route, bool broadcast) =>
            {
                if (mountBags.TryRemove(route, out var bag))
                {
                    bag.TaskCompletionSource.SetResult();
                    bag.Dispose();
                }
                if (broadcast && onMountHandlers.TryGetValue(route, out var handlers))
                {
                    foreach (var handler in handlers)
                    {
                        var ret = handler.Method.Invoke(handler.Target, null);
                        if (ret is Action action)
                        {
                            if (!onceUnmountHandlers.TryGetValue(route, out var onceHandlers))
                            {
                                onceUnmountHandlers[route] = [action];
                            }
                            else
                            {
                                onceHandlers.Add(action);
                            }
                        }
                    }
                }
            }
        );

        (this as IWebView).On(
            "router.unmount",
            (string route, bool broadcast) =>
            {
                if (unmountBags.TryRemove(route, out var bag))
                {
                    bag.TaskCompletionSource.SetResult();
                    bag.Dispose();
                }
                if (broadcast)
                {
                    if (onUnmountHandlers.TryGetValue(route, out var handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            handler();
                        }
                    }
                    if (onceUnmountHandlers.TryRemove(route, out handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            handler();
                        }
                    }
                }
            }
        );
    }

    public void Publish(string eventName, object?[]? args = null)
    {
        Publish(eventName, messageIdProvider.GetNext(), args);
    }

    public void Publish(string eventName, long messageId, object?[]? args = null)
    {
        Emit(eventName, BuildArgs(messageId, args));
    }

    public Task<Effect<object?, GenericError>> SendAsync(string eventName, object?[]? args = null)
    {
        return SendAsync<object?>(eventName, args);
    }

    public Task<Effect<T, GenericError>> SendAsync<T>(string eventName, object?[]? args = null)
    {
        var messageId = messageIdProvider.GetNext();
        if (!sendHandlers.ContainsKey(eventName))
        {
            sendHandlers[eventName] = (this as IWebView).On<long, object?>(eventName, Answer);
        }

        var tcs = new TaskCompletionSource<object?>();
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var ctr = cts.Token.Register(() =>
        {
            tcs.TrySetCanceled();
        });
        _ = tcs.Task.ContinueWith(
            (task, state) =>
            {
                if (state is not long messageId || !sendBags.TryRemove(messageId, out var bag))
                {
                    return;
                }
                bag.Dispose();
            },
            messageId,
            CancellationToken.None,
            TaskContinuationOptions.NotOnRanToCompletion,
            TaskScheduler.Default
        );
        sendBags[messageId] = new StateBag(tcs, cts, ctr);
        Emit(eventName, BuildArgs(messageId, args));
        return MapInternal<T>(tcs.Task);
    }

    public Task<Effect<None, GenericError>> MountAsync(Route route, object? props = null) =>
        MountAsync(route.Value, props);

    public Task<Effect<None, GenericError>> MountAsync(string route, object? props = null)
    {
        if (!mountBags.TryGetValue(route, out var bag))
        {
            var tcs = new TaskCompletionSource();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var ctr = cts.Token.Register(() =>
            {
                tcs.TrySetCanceled();
            });
            _ = tcs.Task.ContinueWith(
                (task, state) =>
                {
                    if (state is not string route || !mountBags.TryRemove(route, out var bag))
                    {
                        return;
                    }
                    bag.Dispose();
                },
                route,
                CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion,
                TaskScheduler.Default
            );
            bag = new SimpleStateBag(tcs, cts, ctr);
            mountBags[route] = bag;
            Emit("router.mount", route);
        }

        return MapMountInternal(bag.TaskCompletionSource.Task);
    }

    public Task<Effect<None, GenericError>> UnmountAsync(Route route, object? props = null) =>
        UnmountAsync(route.Value, props);

    public Task<Effect<None, GenericError>> UnmountAsync(string route, object? props = null)
    {
        if (!unmountBags.TryGetValue(route, out var bag))
        {
            var tcs = new TaskCompletionSource();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var ctr = cts.Token.Register(() =>
            {
                tcs.TrySetCanceled();
            });
            _ = tcs.Task.ContinueWith(
                (task, state) =>
                {
                    if (state is not string route || !unmountBags.TryRemove(route, out var bag))
                    {
                        return;
                    }
                    bag.Dispose();
                },
                route,
                CancellationToken.None,
                TaskContinuationOptions.NotOnRanToCompletion,
                TaskScheduler.Default
            );
            bag = new SimpleStateBag(tcs, cts, ctr);
            unmountBags[route] = bag;
            Emit("router.unmount", route);
        }

        return MapMountInternal(bag.TaskCompletionSource.Task);
    }

    public void ToggleFocus(bool toggle)
    {
        if (toggle)
        {
            if (Interlocked.Increment(ref focusCounter) > 0 && !Focused)
            {
                Focus();
            }
        }
        else
        {
            if (Interlocked.Decrement(ref focusCounter) <= 0 && Focused)
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
                (long messageId) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName));
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
                (long messageId) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName))
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
                (long messageId, T1 arg1) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName), arg1);
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
                (long messageId, T1 arg1) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName), arg1)
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
                (long messageId, T1 arg1, T2 arg2) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName), arg1, arg2);
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
                (long messageId, T1 arg1, T2 arg2) =>
                {
                    handler(uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName), arg1, arg2)
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
                        arg1,
                        arg2,
                        arg3
                    );
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
                            arg1,
                            arg2,
                            arg3
                        )
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
                        arg1,
                        arg2,
                        arg3,
                        arg4
                    );
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
                            arg1,
                            arg2,
                            arg3,
                            arg4
                        )
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                        uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                            uiMessagingContextFactory.CreateMessagingContext(this, messageId, eventName),
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
                (long messageId, T1 arg1) =>
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
                (long messageId, T1 arg1, T2 arg2) =>
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3) =>
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
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
                (long messageId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(arg1, arg2, arg3, arg4, arg5, arg6, arg7)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action OnMount(Route route, Action handler)
    {
        if (!onMountHandlers.TryGetValue(route.Value, out var handlers))
        {
            handlers = [handler];
            onMountHandlers[route.Value] = handlers;
        }
        else
        {
            handlers.Add(handler);
        }
        return () =>
        {
            OffMount(route, handler);
        };
    }

    public Action OnMount(Route route, Func<Action> handler)
    {
        if (!onMountHandlers.TryGetValue(route.Value, out var handlers))
        {
            handlers = [handler];
            onMountHandlers[route.Value] = handlers;
        }
        else
        {
            handlers.Add(handler);
        }
        return () =>
        {
            OffMount(route, handler);
        };
    }

    public Action OnMount(Route route, Func<Task> handler)
    {
        return OnMount(
            route,
            () =>
            {
                handler().SafeFireAndForget(e => Alt.LogError(e.ToString()));
            }
        );
    }

    public Action OnUnmount(Route route, Action handler)
    {
        if (!onUnmountHandlers.TryGetValue(route.Value, out var handlers))
        {
            handlers = [handler];
            onUnmountHandlers[route.Value] = handlers;
        }
        else
        {
            handlers.Add(handler);
        }
        return () =>
        {
            OffUnmount(route, handler);
        };
    }

    public Action OnUnmount(Route route, Func<Task> handler)
    {
        return OnUnmount(
            route,
            () =>
            {
                handler().SafeFireAndForget(e => Alt.LogError(e.ToString()));
            }
        );
    }

    private Action OnInternal(string eventName, Function function)
    {
        return () =>
        {
            this.Off(eventName, function);
        };
    }

    private static async Task<Effect<T, GenericError>> MapInternal<T>(Task<object?> task)
    {
        try
        {
            var ret = await task.ConfigureAwait(false);
            if (ret is not T t)
            {
                return Effect.Fail(GenericError.From(new TypeMismatchError()));
            }
            return Effect.Succeed(t);
        }
        catch (OperationCanceledException)
        {
            return Effect.Fail(GenericError.From(new OperationCanceledError()));
        }
    }

    private static async Task<Effect<None, GenericError>> MapMountInternal(Task task)
    {
        try
        {
            await task.ConfigureAwait(false);
            return Effect.Succeed(new None());
        }
        catch (OperationCanceledException)
        {
            return Effect.Fail(GenericError.From(new OperationCanceledError()));
        }
    }

    private void Answer(long messageId, object? answer)
    {
        if (!sendBags.TryRemove(messageId, out var bag))
        {
            return;
        }

        bag.TaskCompletionSource.TrySetResult(answer);
        bag.Dispose();
    }

    private static object?[] BuildArgs(long messageId, object?[]? args = null)
    {
        if (args is null)
        {
            return [messageId];
        }

        var arr = new object?[args.Length + 1];
        arr[0] = messageId;
        Array.Copy(args, 0, arr, 1, args.Length);
        return arr;
    }

    private void OffMount(Route route, Delegate handler)
    {
        if (!onMountHandlers.TryGetValue(route.Value, out var handlers))
        {
            return;
        }
        handlers.Remove(handler);
    }

    private void OffUnmount(Route route, Action handler)
    {
        if (!onUnmountHandlers.TryGetValue(route.Value, out var handlers))
        {
            return;
        }
        handlers.Remove(handler);
    }

    private class StateBag(
        TaskCompletionSource<object?> taskCompletionSource,
        CancellationTokenSource cancellationTokenSource,
        CancellationTokenRegistration cancellationTokenRegistration
    )
    {
        public readonly TaskCompletionSource<object?> TaskCompletionSource = taskCompletionSource;
        public readonly CancellationTokenSource CancellationTokenSource = cancellationTokenSource;
        public readonly CancellationTokenRegistration CancellationTokenRegistration = cancellationTokenRegistration;

        public void Dispose()
        {
            CancellationTokenRegistration.Dispose();
            CancellationTokenSource.Dispose();
        }
    }

    private class SimpleStateBag(
        TaskCompletionSource taskCompletionSource,
        CancellationTokenSource cancellationTokenSource,
        CancellationTokenRegistration cancellationTokenRegistration
    )
    {
        public readonly TaskCompletionSource TaskCompletionSource = taskCompletionSource;
        public readonly CancellationTokenSource CancellationTokenSource = cancellationTokenSource;
        public readonly CancellationTokenRegistration CancellationTokenRegistration = cancellationTokenRegistration;

        public void Dispose()
        {
            CancellationTokenRegistration.Dispose();
            CancellationTokenSource.Dispose();
        }
    }
}
