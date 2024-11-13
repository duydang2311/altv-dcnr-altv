using AltV.Community.Events;

namespace CnR.Server.Features.Lobbies.Pursuits.Abstractions;

public sealed class PursuitLobbyStatusChangedEvent(IEventInvoker invoker)
    : Event<IPursuitLobby, PursuitLobbyStatus, PursuitLobbyStatus>(invoker) { }
