using AltV.Net.Client;
using CnR.Client.Common;
using CnR.Client.Features.Uis.Abstractions;

namespace CnR.Client.Features.Accounts.Scripts;

public sealed class SignInScript(IUi ui) : Script
{
    private const string DiscordAppId = "1303683973410455674";

    public override Task StartAsync(CancellationToken ct)
    {
        ui.On("sign-in.discord", OnSignInDiscordAsync);
        Alt.OnConnectionComplete += OnConnectionComplete;
        return Task.CompletedTask;
    }

    private void OnConnectionComplete()
    {
        ui.Emit("router.mount", "sign_in");
        ui.Focus();
        Alt.ShowCursor(true);
        Alt.Natives.TriggerScreenblurFadeIn(0);
        Alt.Natives.DisplayHud(false);
        Alt.Natives.DisplayRadar(false);
        Alt.GameControlsEnabled = false;
    }

    private async Task OnSignInDiscordAsync()
    {
        string? bearerToken;
        try
        {
            bearerToken = await Alt.Discord.RequestOAuth2Token(DiscordAppId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

        Alt.EmitServer("sign-in.discord", bearerToken);
    }
}
