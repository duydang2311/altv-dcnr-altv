using System.Reflection;
using AltV.Community.Events;
using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Infrastructure.Persistence.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CnR.Server;

public sealed class ServerResource : AsyncResource
{
    private readonly IHost host;

    public ServerResource()
    {
        var builder = Host.CreateDefaultBuilder();

        builder.UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
        builder.ConfigureAppConfiguration(b =>
        {
            b.Sources.Clear();
            b.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            b.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false);
        });
        builder.ConfigureServices(
            (context, services) =>
            {
                services
                    .AddHttpClient()
                    .AddSingleton<IHostLifetime, ServerResourceLifetime>()
                    .AddPersistence(
                        context.Configuration.GetSection(DbOptions.Section).Get<DbOptions>()
                            ?? throw new InvalidOperationException(
                                "Missing configuration for \"Db\" section in appsettings"
                            )
                    )
                    .AddSingleton<IEventInvoker, EventInvoker>()
                    .AddAccountFeatures()
                    .AddCharacterFeatures()
                    .AddMessagingFeatures()
                    .AddUiFeatures()
                    .AddPursuitLobbyFeatures()
                    .AddLobbyFeatures();
            }
        );

        host = builder.Build();
    }

    public override void OnStart()
    {
        StartAsync().Wait();
    }

    public override void OnStop()
    {
        StopAsync().Wait();
    }

    public override IEntityFactory<IPlayer> GetPlayerFactory()
    {
        return host.Services.GetRequiredService<ICharacterFactory>();
    }

    private async Task StartAsync()
    {
        AltExtensions.RegisterAdapters(registerListAdapters: true);
        await host.StartAsync().ConfigureAwait(false);
    }

    private async Task StopAsync()
    {
        await host.StopAsync().ConfigureAwait(false);
        host.Dispose();
    }

    private sealed class ServerResourceLifetime : IHostLifetime
    {
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
