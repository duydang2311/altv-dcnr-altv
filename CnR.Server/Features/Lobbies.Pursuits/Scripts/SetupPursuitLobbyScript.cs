using System.Collections.Concurrent;
using AltV.Community.Messaging.Server.Abstractions;
using AltV.Net;
using AsyncAwaitBestPractices;
using CnR.Server.Common;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits.Abstractions;
using CnR.Shared.Dtos;

namespace CnR.Server.Features.Lobbies.Pursuits.Scripts;

public sealed class SetupPursuitLobbyScript(ILobbyCreatedEvent lobbyCreatedEvent, IMessenger messenger) : Script
{
    private readonly ConcurrentDictionary<ILobby, Timer> countdownTimers = [];

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        lobbyCreatedEvent.AddHandler(OnLobbyCreated);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var timer in countdownTimers)
        {
            await timer.Value.DisposeAsync().ConfigureAwait(false);
        }
        countdownTimers.Clear();
    }

    private void OnLobbyCreated(ILobby lobby)
    {
        if (lobby is not IPursuitLobby pursuitLobby)
        {
            return;
        }

        pursuitLobby.PlayerAddedEvent.AddHandler(OnPlayerAdded);
        pursuitLobby.PlayerRemovedEvent.AddHandler(OnPlayerRemoved);
        pursuitLobby.StatusChangedEvent.AddHandler(OnStatusChanged);
        pursuitLobby.Status = PursuitLobbyStatus.Open;
    }

    private void OnPlayerAdded(ILobby lobby, ICharacter character)
    {
        if (lobby is not IPursuitLobby pursuitLobby)
        {
            return;
        }

        switch (pursuitLobby.Status)
        {
            case PursuitLobbyStatus.Open:
                if (pursuitLobby.GetPlayers().Count > 3)
                {
                    pursuitLobby.Status = PursuitLobbyStatus.Countdown;
                }
                break;
            case PursuitLobbyStatus.Countdown:
                break;
        }
    }

    private void OnPlayerRemoved(ILobby lobby, ICharacter character) { }

    private async Task OnStatusChanged(IPursuitLobby lobby, PursuitLobbyStatus previous, PursuitLobbyStatus current)
    {
        switch (previous)
        {
            case PursuitLobbyStatus.Countdown:
                await RemoveCountdownTimerAsync(lobby).ConfigureAwait(false);
                break;
        }

        switch (current)
        {
            case PursuitLobbyStatus.Countdown:
                StartCountdown(lobby);
                break;
            case PursuitLobbyStatus.Ongoing:
                StartOngoing(lobby);
                break;
        }
    }

    private void StartCountdown(IPursuitLobby lobby)
    {
        var duration = TimeSpan.FromSeconds(16);
        countdownTimers[lobby] = new Timer(
            (state) => OnCountdownAsync((IPursuitLobby)state!).SafeFireAndForget(e => Alt.LogError(e.ToString())),
            lobby,
            duration,
            Timeout.InfiniteTimeSpan
        );
        messenger.Publish(
            lobby.GetPlayers(),
            "pursuit-lobby.countdown",
            [new PursuitLobbyCountdownDto { DurationSeconds = (int)duration.TotalSeconds }]
        );
    }

    private void StartOngoing(IPursuitLobby lobby)
    {
        messenger.Publish(lobby.GetPlayers(), "pursuit-lobby.ongoing");
    }

    private async Task OnCountdownAsync(IPursuitLobby lobby)
    {
        if (countdownTimers.TryRemove(lobby, out var timer))
        {
            await timer.DisposeAsync().ConfigureAwait(false);
        }
        lobby.Status = PursuitLobbyStatus.Ongoing;
    }

    private async ValueTask RemoveCountdownTimerAsync(IPursuitLobby lobby)
    {
        if (countdownTimers.TryRemove(lobby, out var timer))
        {
            await timer.DisposeAsync().ConfigureAwait(false);
        }
    }
}
