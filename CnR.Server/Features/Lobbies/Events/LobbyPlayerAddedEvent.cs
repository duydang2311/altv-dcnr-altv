using AltV.Community.Events;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Features.Lobbies.Abstractions;

namespace CnR.Server.Features.Lobbies.Events;

public sealed class LobbyPlayerAddedEvent(IEventInvoker invoker)
    : Event<ILobby, ICharacter>(invoker),
        ILobbyPlayerAddedEvent { }
