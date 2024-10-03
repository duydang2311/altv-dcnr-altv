using System.Reflection;
using AltV.Net.Async;
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
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IHostLifetime, ServerResourceLifetime>();
        });

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

    private async Task StartAsync()
    {
        await host.StartAsync().ConfigureAwait(false);
    }

    private async Task StopAsync()
    {
        await host.StopAsync().ConfigureAwait(false);
        host.Dispose();
    }

    private class ServerResourceLifetime : IHostLifetime
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
