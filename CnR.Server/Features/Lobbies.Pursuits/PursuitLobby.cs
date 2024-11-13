using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;

namespace CnR.Server.Features.Lobbies.Pursuits;

public sealed class PursuitLobby(
    PursuitLobbyOptions options,
    ILobbyPlayerAddedEvent lobbyPlayerAddedEvent,
    ILobbyPlayerRemovedEvent lobbyPlayerRemovedEvent,
    PursuitLobbyStatusChangedEvent pursuitLobbyStatusChangedEvent
) : BaseLobby(options, lobbyPlayerAddedEvent, lobbyPlayerRemovedEvent), IPursuitLobby
{
    private PursuitLobbyStatus status;

    public PursuitLobbyStatus Status
    {
        get => status;
        set
        {
            var old = status;
            status = value;
            if (status != old)
            {
                StatusChangedEvent.Invoke(this, old, status);
            }
        }
    }
    public PursuitLobbyStatusChangedEvent StatusChangedEvent => pursuitLobbyStatusChangedEvent;
}
