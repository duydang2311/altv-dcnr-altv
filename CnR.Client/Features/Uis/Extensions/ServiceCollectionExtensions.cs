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
            .AddSingleton(provider => (IUi)Alt.CreateWebView("http://localhost:3000"));
        return serviceCollection;
    }
}
