using AltV.Community.Events;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Features.Lobbies.Abstractions;

namespace CnR.Server.Features.Lobbies.Events;

public sealed class LobbyPlayerRemovedEvent(IEventInvoker invoker)
    : Event<ILobby, ICharacter>(invoker),
        ILobbyPlayerRemovedEvent { }
