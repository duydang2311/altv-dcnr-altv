using System.Net.Http.Json;
using System.Text.Json;
using AltV.Community.Messaging.Server.Abstractions;
using CnR.Server.Common;
using CnR.Server.Domain.Models;
using CnR.Server.Features.Accounts.Abstractions;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Features.Messaging.Abstractions;
using CnR.Server.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CnR.Server.Features.Accounts.Scripts;

public sealed class SignInScript(
    IHttpClientFactory httpClientFactory,
    IDbFactory dbFactory,
    IAccountLoggedInEvent accountLoggedInEvent,
    IEffectfulMessenger messenger
) : Script
{
    private const string DiscordApiCurrentUserEndpoint = "https://discordapp.com/api/users/@me";
    private readonly JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web);

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        messenger.On<IAltCharacter, string>("sign-in.discord.confirm", OnSignInDiscordConfirmAsync);
        return Task.CompletedTask;
    }

    private async Task OnSignInDiscordConfirmAsync(IMessagingContext<IAltCharacter> ctx, string bearerToken)
    {
        var getDiscordUser = await GetDiscordUserAsync(bearerToken).ConfigureAwait(false);
        if (getDiscordUser.TryGetError(out var e, out var user))
        {
            ctx.Respond("oauth_failed");
            return;
        }

        await using var db = await dbFactory.CreateDbAsync().ConfigureAwait(false);
        var acc = await db
            .DiscordAccounts.Where(a => a.DiscordId.Equals(user.Id))
            .Select(a => new { a.AccountId })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (acc is not null)
        {
            ctx.Player.AccountId = acc.AccountId;
            ctx.Respond("success");
            return;
        }

        var account = new Account
        {
            Discord = new DiscordAccount
            {
                DiscordId = user.Id,
                DiscordUsername = user.Username,
                DiscordAvatar = user.Avatar,
            },
        };
        db.Accounts.Add(account);

        var saveEffect = await Try(() => db.SaveChangesAsync())().ConfigureAwait(false);
        if (saveEffect.TryGetError(out e, out var count) || count == 0)
        {
            ctx.Respond("register_failed");
            return;
        }

        ctx.Player.AccountId = account.Id;
        accountLoggedInEvent.Invoke(ctx.Player);
        ctx.Respond("success");
    }

    private async Task<Effect<DiscordUser, GenericError>> GetDiscordUserAsync(string bearerToken)
    {
        using var httpClient = httpClientFactory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, DiscordApiCurrentUserEndpoint);
        request.Headers.Add("Authorization", $"Bearer {bearerToken}");

        var send = await Try(() => httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))()
            .ConfigureAwait(false);
        if (send.TryGetError(out var e, out var sendSuccess))
        {
            return Effect.Fail(e);
        }

        using var response = sendSuccess;
        if (!response.IsSuccessStatusCode)
        {
            return Effect.Fail(GenericError.From("..."));
        }

        var read = await Try(() => response.Content.ReadFromJsonAsync<DiscordUser>(serializerOptions))()
            .ConfigureAwait(false);
        if (read.TryGetError(out e, out var user))
        {
            return Effect.Fail(e);
        }
        return Effect.Succeed(user);
    }

    private sealed record DiscordUser
    {
        public string Id { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string? Avatar { get; init; }
        public string? Banner { get; init; }
    }
}
