using AltV.Community.Events;
using CnR.Server.Features.Characters.Abstractions;

namespace CnR.Server.Features.Lobbies.Abstractions;

public interface ILobbyPlayerRemovedEvent : IEvent<ILobby, ICharacter> { }
