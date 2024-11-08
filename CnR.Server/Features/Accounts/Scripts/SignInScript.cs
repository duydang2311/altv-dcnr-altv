using System.Net.Http.Json;
using System.Text.Json;
using AltV.Net.Async;
using CnR.Server.Common;
using CnR.Server.Domain.Models;
using CnR.Server.Infrastructure.Persistence.Abstractions;
using CnR.Server.Players.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CnR.Server.Features.Accounts.Scripts;

public sealed class SignInScript(IHttpClientFactory httpClientFactory, IDbFactory dbFactory) : Script
{
    private const string DiscordApiCurrentUserEndpoint = "https://discordapp.com/api/users/@me";
    private readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        AltAsync.OnClient<IAltCharacter, string, Task>("sign-in.discord", OnSignInDiscordAsync);
        return Task.CompletedTask;
    }

    private async Task OnSignInDiscordAsync(IAltCharacter character, string bearerToken)
    {
        using var httpClient = httpClientFactory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, DiscordApiCurrentUserEndpoint);
        request.Headers.Add("Authorization", $"Bearer {bearerToken}");

        var sendEffect = await Try(() => httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))()
            .ConfigureAwait(false);
        if (sendEffect.TryGetError(out var e, out var sendSuccess))
        {
            return;
        }

        using var response = sendSuccess;
        if (!response.IsSuccessStatusCode)
        {
            return;
        }

        var readEffect = await Try(() => response.Content.ReadFromJsonAsync<DiscordUser>(SerializerOptions))()
            .ConfigureAwait(false);
        if (readEffect.TryGetError(out e, out var user))
        {
            return;
        }

        await using var db = await dbFactory.CreateDbAsync().ConfigureAwait(false);

        var any = await db.DiscordAccounts.AnyAsync(a => a.DiscordId.Equals(user.Id)).ConfigureAwait(false);
        if (any)
        {
            return;
        }

        db.Accounts.Add(new Account
        {
            Discord = new DiscordAccount
            {
                DiscordId = user.Id,
                DiscordUsername = user.Username,
                DiscordAvatar = user.Avatar,
            },
        });

        var saveEffect = await Try(() => db.SaveChangesAsync())().ConfigureAwait(false);
        if (saveEffect.TryGetError(out e, out var count) || count == 0)
        {
            return;
        }
    }

    private sealed record DiscordUser
    {
        public string Id { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string? Avatar { get; init; }
        public string? Banner { get; init; }
    }
}
