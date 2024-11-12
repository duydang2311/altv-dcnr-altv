using AltV.Community.Messaging;
using AltV.Community.Messaging.Abstractions;
using AltV.Net.Client;
using CnR.Client.Features.Uis;
using CnR.Client.Features.Uis.Abstractions;
using CnR.Client.Features.Uis.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddUiFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddKeyedSingleton<IMessageIdProvider, MessageIdProvider>("ui")
            .AddSingleton<IUiFactory, UiFactory>()
            .AddSingleton<IUiMessagingContextFactory, UiMessagingContextFactory>()
            .AddSingleton(provider => (IUi)Alt.CreateWebView("http://localhost:5173"))
            .AddScript<UiScript>();
        return serviceCollection;
    }
}
