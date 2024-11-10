using System.Collections.Concurrent;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using CnR.Server.Features.Messaging.Abstractions;
using CnR.Shared.Errors;

namespace CnR.Server.Features.Messaging;

public sealed class Messenger(IMessagingContextFactory messagingContextFactory) : IMessenger
{
    private readonly ConcurrentDictionary<(IPlayer, string), StateBag> messageTasks = [];

    private readonly ConcurrentDictionary<string, Function> handlerTasks = [];

    public void Publish(IPlayer player, string eventName, object?[]? args = null)
    {
        player.Emit(eventName, args ?? []);
    }

    public Task<Effect<object?, GenericError>> SendAsync(IPlayer player, string eventName, object?[]? args = null)
    {
        return SendAsync<object?>(player, eventName, args);
    }

    public Task<Effect<T, GenericError>> SendAsync<T>(IPlayer player, string eventName, object?[]? args = null)
    {
        var bag = SendAsyncInternal(player, eventName, args);
        return MapInternal<T>(bag.TaskCompletionSource.Task);
    }

    public bool Answer(IPlayer player, string eventName, object? answer)
    {
        var key = (player, eventName);
        if (!messageTasks.TryRemove(key, out var bag))
        {
            return false;
        }

        bag.TaskCompletionSource.SetResult(answer);
        bag.TaskCompletionSource.Task.Dispose();
        bag.CancellationTokenRegistration.Dispose();
        bag.CancellationTokenSource.Dispose();
        return true;
    }

    private StateBag SendAsyncInternal(IPlayer player, string eventName, object?[]? args)
    {
        var key = (player, eventName);
        if (!messageTasks.TryGetValue(key, out var bag))
        {
            if (!handlerTasks.TryGetValue(eventName, out var f))
            {
                f = Alt.OnClient<IPlayer, object?>(
                    eventName,
                    (player, answer) =>
                    {
                        Answer(player, eventName, answer);
                    }
                );
                handlerTasks[eventName] = f;
            }

            var tcs = new TaskCompletionSource<object?>();
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var reg = cts.Token.Register(() =>
            {
                Answer(player, eventName, new OperationCanceledError());
            });
            bag = new StateBag(tcs, cts, reg);
            messageTasks[key] = bag;
            player.Emit(eventName, args);
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

    public Action On<TPlayer>(string eventName, Action<IMessagingContext<TPlayer>> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient<TPlayer>(
                eventName,
                (player) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(player, eventName));
                }
            )
        );
    }

    public Action On<TPlayer>(string eventName, Func<IMessagingContext<TPlayer>, Task> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player) =>
                {
                    return handler(messagingContextFactory.CreateMessagingContext(player, eventName));
                }
            )
        );
    }

    public Action On<TPlayer, T1>(string eventName, Action<IMessagingContext<TPlayer>, T1> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1);
                }
            )
        );
    }

    public Action On<TPlayer, T1>(string eventName, Func<IMessagingContext<TPlayer>, T1, Task> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1) =>
                {
                    return handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1);
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2>(string eventName, Action<IMessagingContext<TPlayer>, T1, T2> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1, arg2);
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2>(string eventName, Func<IMessagingContext<TPlayer>, T1, T2, Task> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2) =>
                {
                    return handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1, arg2);
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2, T3>(string eventName, Action<IMessagingContext<TPlayer>, T1, T2, T3> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1, arg2, arg3);
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2, T3>(string eventName, Func<IMessagingContext<TPlayer>, T1, T2, T3, Task> handler)
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3) =>
                {
                    return handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1, arg2, arg3);
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2, T3, T4>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    handler(messagingContextFactory.CreateMessagingContext(player, eventName), arg1, arg2, arg3, arg4);
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2, T3, T4>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, Task> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
                {
                    return handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
                        arg1,
                        arg2,
                        arg3,
                        arg4
                    );
                }
            )
        );
    }

    public Action On<TPlayer, T1, T2, T3, T4, T5>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
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

    public Action On<TPlayer, T1, T2, T3, T4, T5>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, Task> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
                {
                    return handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
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

    public Action On<TPlayer, T1, T2, T3, T4, T5, T6>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
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

    public Action On<TPlayer, T1, T2, T3, T4, T5, T6>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6, Task> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
                {
                    return handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
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

    public Action On<TPlayer, T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Action<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6, T7> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            Alt.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
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

    public Action On<TPlayer, T1, T2, T3, T4, T5, T6, T7>(
        string eventName,
        Func<IMessagingContext<TPlayer>, T1, T2, T3, T4, T5, T6, T7, Task> handler
    )
        where TPlayer : IPlayer
    {
        return OnInternal(
            eventName,
            AltAsync.OnClient(
                eventName,
                (TPlayer player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
                {
                    return handler(
                        messagingContextFactory.CreateMessagingContext(player, eventName),
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
