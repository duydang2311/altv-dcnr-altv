using CnR.Server.Features.Lobbies.Abstractions;

namespace CnR.Server.Features.Lobbies.Pursuits.Abstractions;

public interface IPursuitLobby : ILobby
{
    PursuitLobbyStatusChangedEvent StatusChangedEvent { get; }
    PursuitLobbyStatus Status { get; set; }
}
