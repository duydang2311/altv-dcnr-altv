using AltV.Net;
using AltV.Net.Client;

namespace CnR.Client.Features.Games.Abstractions;

public static class GameExtensions
{
    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7, T8> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6, T7> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5, T6> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5>(
        this IGame game,
        string eventName,
        Action<T1, T2, T3, T4, T5> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4>(this IGame game, string eventName, Action<T1, T2, T3, T4> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3>(this IGame game, string eventName, Action<T1, T2, T3> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2>(this IGame game, string eventName, Action<T1, T2> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1>(this IGame game, string eventName, Action<T1> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer(this IGame game, string eventName, Action function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7, T8> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6, T7>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6, T7> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5, T6>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5, T6> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4, T5>(
        this IGame game,
        string eventName,
        Func<T1, T2, T3, T4, T5> function
    ) => OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3, T4>(this IGame game, string eventName, Func<T1, T2, T3, T4> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2, T3>(this IGame game, string eventName, Func<T1, T2, T3> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1, T2>(this IGame game, string eventName, Func<T1, T2> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    public static Action OnServer<T1>(this IGame game, string eventName, Func<T1> function) =>
        OnServer(eventName, Alt.CreateFunction(function));

    private static Action OnServer(string eventName, Function function)
    {
        var f = Alt.OnServer(eventName, function);
        return () =>
        {
            Alt.OffServer(eventName, f);
        };
    }
}
