using AltV.Net.Client;
using AltV.Net.Client.Async;
using AltV.Net.Client.Elements.Interfaces;
using CnR.Client.Common.Abstractions;
using CnR.Client.Features.Uis.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CnR.Client;

public sealed class ClientResource : AsyncResource
{
    private readonly IServiceProvider serviceProvider;

    public ClientResource()
    {
        var serviceCollection = new ServiceCollection().AddUiFeatures().AddAccountFeatures();

        serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public override void OnStart()
    {
        StartAsync().Wait();
    }

    public override IBaseObjectFactory<IWebView> GetWebViewFactory()
    {
        return serviceProvider.GetRequiredService<IUiFactory>();
    }

    public override void OnStop() { }

    private Task StartAsync()
    {
        return Task.WhenAll(
            serviceProvider
                .GetServices<IHostedService>()
                .Select(a => a.StartAsync(CancellationToken.None))
        );
    }
}
