using AltV.Community.Messaging.Client;
using AltV.Community.Messaging.Client.Abstractions;
using CnR.Client.Features.Messaging;
using CnR.Client.Features.Messaging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMessagingContextFactory, MessagingContextFactory>()
            .AddSingleton<IMessenger>(provider => provider.GetRequiredService<IEffectfulMessenger>())
            .AddSingleton<IEffectfulMessenger, EffectfulMessenger>();
        return serviceCollection;
    }
}
