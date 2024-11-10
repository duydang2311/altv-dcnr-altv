using CnR.Client.Features.Messaging;
using CnR.Client.Features.Messaging.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessagingFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMessagingContextFactory, MessagingContextFactory>()
            .AddSingleton<IMessenger, Messenger>();
        return serviceCollection;
    }
}
