using System.Net.Http.Json;
using System.Text.Json;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using CnR.Server.Common;
using CnR.Server.Domain.Models;
using CnR.Server.Features.Accounts.Abstractions;
using CnR.Server.Infrastructure.Persistence.Abstractions;
using CnR.Server.Players.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CnR.Server.Features.Accounts.Scripts;

public sealed class SignInScript(
    IHttpClientFactory httpClientFactory,
    IDbFactory dbFactory,
    IAccountLoggedInEvent accountLoggedInEvent
) : Script
{
    private const string DiscordApiCurrentUserEndpoint = "https://discordapp.com/api/users/@me";
    private readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        AltAsync.OnClient<IAltCharacter, string, Task>(
            "sign-in.discord.confirm",
            OnSignInDiscordConfirmAsync
        );
        return Task.CompletedTask;
    }

    private async Task OnSignInDiscordConfirmAsync(IAltCharacter character, string bearerToken)
    {
        var getDiscordUser = await GetDiscordUserAsync(bearerToken).ConfigureAwait(false);
        if (getDiscordUser.TryGetError(out var e, out var user))
        {
            character.Emit("sign-in.discord.confirm", "oauth_failed");
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
            character.AccountId = acc.AccountId;
            character.Emit("sign-in.discord.confirm", "success");
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
            character.Emit("sign-in.discord.confirm", "register_failed");
            return;
        }

        character.AccountId = account.Id;
        character.Emit("sign-in.discord.confirm", "success");
        accountLoggedInEvent.Invoke(character);
    }

    private async Task<Effect<DiscordUser, GenericError>> GetDiscordUserAsync(string bearerToken)
    {
        using var httpClient = httpClientFactory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, DiscordApiCurrentUserEndpoint);
        request.Headers.Add("Authorization", $"Bearer {bearerToken}");

        var send = await Try(
            () => httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
        )()
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

        var read = await Try(
            () => response.Content.ReadFromJsonAsync<DiscordUser>(SerializerOptions)
        )()
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
