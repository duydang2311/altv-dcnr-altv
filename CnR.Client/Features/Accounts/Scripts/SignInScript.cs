using System.Net.Http.Json;
using System.Text.Json;
using AltV.Community.Messaging.Client.Abstractions;
using AltV.Net.Client;
using CnR.Client.Common;
using CnR.Client.Features.Messaging.Abstractions;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Shared.Dtos;
using CnR.Shared.Errors;

namespace CnR.Client.Features.Accounts.Scripts;

public sealed class SignInScript(IUi ui, IEffectfulMessenger messenger) : Script
{
    private const string DiscordAppId = "1303683973410455674";
    private const string DiscordApiCurrentUserEndpoint = "https://discordapp.com/api/users/@me";
    private readonly JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web);
    private string? bearerToken;

    public override Task StartAsync(CancellationToken ct)
    {
        ui.On("sign-in.discord.request", OnUiSignInDiscordRequestAsync);
        ui.On("sign-in.discord.confirm", OnUiSignInDiscordConfirmAsync);
        Alt.OnConnectionComplete += OnConnectionComplete;
        return Task.CompletedTask;
    }

    private void OnConnectionComplete()
    {
        ui.Publish("router.mount", ["sign_in"]);
        ui.ToggleFocus(true);
        Alt.ShowCursor(true);
        Alt.Natives.TriggerScreenblurFadeIn(0);
        Alt.Natives.DisplayHud(false);
        Alt.Natives.DisplayRadar(false);
        Alt.GameControlsEnabled = false;
    }

    private async Task OnUiSignInDiscordRequestAsync(IMessagingContext ctx)
    {
        try
        {
            bearerToken = await Alt.Discord.RequestOAuth2Token(DiscordAppId).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

        var getDiscordUser = await GetDiscordUserAsync(bearerToken).ConfigureAwait(false);
        if (getDiscordUser.TryGetError(out var error, out var user))
        {
            Console.WriteLine(error);
            return;
        }

        ctx.Respond(
            new SignInDiscordRequestDto
            {
                Id = user.Id,
                Username = user.Username,
                Avatar = user.Avatar,
                Banner = user.Banner
            }
        );
    }

    private async Task<Effect<DiscordUser, GenericError>> GetDiscordUserAsync(string bearerToken)
    {
        using var httpClient = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, DiscordApiCurrentUserEndpoint);
        request.Headers.Add("Authorization", $"Bearer {bearerToken}");

        var sendEffect = await Try(() => httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))()
            .ConfigureAwait(false);
        if (sendEffect.TryGetError(out var e, out var sendSuccess))
        {
            return Effect.Fail(e);
        }

        using var response = sendSuccess;
        if (!response.IsSuccessStatusCode)
        {
            return Effect.Fail(GenericError.From("..."));
        }

        var readEffect = await Try(() => response.Content.ReadFromJsonAsync<DiscordUser>(serializerOptions))()
            .ConfigureAwait(false);
        if (readEffect.TryGetError(out e, out var user))
        {
            return Effect.Fail(GenericError.From(e));
        }
        return Effect.Succeed(user);
    }

    private async Task OnUiSignInDiscordConfirmAsync(IMessagingContext ctx)
    {
        var send = await messenger.SendAsync<string>("sign-in.discord.confirm", [bearerToken]).ConfigureAwait(false);
        if (send.TryGetError(out var e, out var success))
        {
            if (e.TryCatch<OperationCanceledError>(out _))
            {
                ctx.Respond("operation_canceled_error");
            }
            else if (e.TryCatch<TypeMismatchError>(out _))
            {
                ctx.Respond("type_mismatch_error");
            }
        }
        ctx.Respond(success);
    }

    private sealed record DiscordUser
    {
        public string Id { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string? Avatar { get; init; }
        public string? Banner { get; init; }
    }
}
