using AltV.Community.Events;
using CnR.Server.Features.Lobbies.Abstractions;

namespace CnR.Server.Features.Lobbies.Events;

public sealed class LobbyCreatedEvent(IEventInvoker invoker) : Event<ILobby>(invoker), ILobbyCreatedEvent { }
