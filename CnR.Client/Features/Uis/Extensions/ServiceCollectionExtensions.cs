using AltV.Net.Client;
using CnR.Client.Features.Uis;
using CnR.Client.Features.Uis.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddUiFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IUiFactory, UiFactory>()
            .AddSingleton<IUiMessagingContextFactory, UiMessagingContextFactory>()
            .AddSingleton(provider => (IUi)Alt.CreateWebView("http://localhost:5173"));
        return serviceCollection;
    }
}
