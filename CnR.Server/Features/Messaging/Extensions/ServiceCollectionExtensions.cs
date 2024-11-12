using AltV.Community.Messaging;
using AltV.Community.Messaging.Abstractions;
using AltV.Community.Messaging.Server;
using AltV.Community.Messaging.Server.Abstractions;
using CnR.Server.Features.Messaging;
using CnR.Server.Features.Messaging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMessagingContextFactory, MessagingContextFactory>()
            .AddSingleton<IMessageIdProvider, MessageIdProvider>()
            .AddSingleton<IMessenger>(provider => provider.GetRequiredService<IEffectfulMessenger>())
            .AddSingleton<IEffectfulMessenger, EffectfulMessenger>();
        return serviceCollection;
    }
}
