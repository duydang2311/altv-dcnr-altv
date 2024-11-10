using System.Collections.Concurrent;
using AltV.Net;
using AltV.Net.Client;
using AsyncAwaitBestPractices;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Shared.Errors;

namespace CnR.Client.Features.Messaging;

public sealed class Messenger(IMessagingContextFactory messagingContextFactory) : IMessenger
{
    private readonly ConcurrentDictionary<string, StateBag> messageTasks = [];

    private readonly ConcurrentDictionary<string, Function> handlerTasks = [];

    public void Publish(string eventName, object?[]? args = null)
    {
        Alt.EmitServer(eventName, args ?? []);
    }

    public Task<Effect<object?, GenericError>> SendAsync(string eventName, object?[]? args = null)
    {
        return SendAsync<object?>(eventName, args);
    }

    public Task<Effect<T, GenericError>> SendAsync<T>(string eventName, object?[]? args = null)
    {
        var bag = SendAsyncInternal(eventName, args);
        return MapInternal<T>(bag.TaskCompletionSource.Task);
    }

    public bool Answer(string eventName, object? answer)
    {
        if (!messageTasks.TryRemove(eventName, out var bag))
        {
            return false;
        }

        bag.TaskCompletionSource.SetResult(answer);
        bag.TaskCompletionSource.Task.Dispose();
        bag.CancellationTokenRegistration.Dispose();
        bag.CancellationTokenSource.Dispose();
        return true;
    }

    private StateBag SendAsyncInternal(string eventName, object?[]? args)
    {
        if (!messageTasks.TryGetValue(eventName, out var bag))
        {
            if (!handlerTasks.TryGetValue(eventName, out var f))
            {
                f = Alt.OnServer<object?>(
                    eventName,
                    (answer) =>
                    {
                        Answer(eventName, answer);
                    }
                );
                handlerTasks[eventName] = f;
            }

            var tcs = new TaskCompletionSource<object?>();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var reg = cts.Token.Register(() =>
            {
                Answer(eventName, new OperationCanceledError());
            });
            bag = new StateBag(tcs, cts, reg);
            messageTasks[eventName] = bag;
            Alt.EmitServer(eventName, args);
        }
        return bag;
    }

    private static async Task<Effect<T, GenericError>> MapInternal<T>(Task<object?> task)
    {
        return await task.ConfigureAwait(false) switch
        {
            OperationCanceledError e => Effect.Fail(GenericError.From(e)),
            T t => Effect.Succeed(t),
            _ => Effect.Fail(GenericError.From(new TypeMismatchError())),
        };
    }

    private static Action OnInternal(string eventName, Function function)
    {
        return () =>
        {
            Alt.OffClient(eventName, function);
        };
    }

    public Action On(string eventName, Action<IMessagingContext> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                () =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName));
                }
            )
        );
    }

    public Action On(string eventName, Func<IMessagingContext, Task> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                () =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName))
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1>(string eventName, Action<IMessagingContext, T1> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1);
                }
            )
        );
    }

    public Action On<T1>(string eventName, Func<IMessagingContext, T1, Task> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2>(string eventName, Action<IMessagingContext, T1, T2> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2);
                }
            )
        );
    }

    public Action On<T1, T2>(string eventName, Func<IMessagingContext, T1, T2, Task> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3>(string eventName, Action<IMessagingContext, T1, T2, T3> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2, arg3);
                }
            )
        );
    }

    public Action On<T1, T2, T3>(string eventName, Func<IMessagingContext, T1, T2, T3, Task> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2, arg3)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4>(string eventName, Action<IMessagingContext, T1, T2, T3, T4> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2, arg3, arg4);
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, Task> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2, arg3, arg4)
                        .SafeFireAndForget(e => Alt.LogError(e.ToString()));
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5>(string eventName, Action<IMessagingContext, T1, T2, T3, T4, T5> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2, arg3, arg4, arg5);
                }
            )
        );
    }

    public Action On<T1, T2, T3, T4, T5>(string eventName, Func<IMessagingContext, T1, T2, T3, T4, T5, Task> handler)
    {
        return OnInternal(
            eventName,
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(eventName), arg1, arg2, arg3, arg4, arg5)
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
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                        messagingContextFactory.CreateMessagingContext(eventName),
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
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                            messagingContextFactory.CreateMessagingContext(eventName),
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
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                        messagingContextFactory.CreateMessagingContext(eventName),
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
            Alt.OnServer(
                eventName,
                (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                            messagingContextFactory.CreateMessagingContext(eventName),
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

    private class StateBag(
        TaskCompletionSource<object?> tcs,
        CancellationTokenSource cts,
        CancellationTokenRegistration ctr
    )
    {
        public readonly TaskCompletionSource<object?> TaskCompletionSource = tcs;
        public readonly CancellationTokenSource CancellationTokenSource = cts;
        public readonly CancellationTokenRegistration CancellationTokenRegistration = ctr;
    }
}
