using CnR.Client.Common.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddScript<TImplementation>(
        this IServiceCollection serviceCollection
    )
        where TImplementation : class, IHostedService
    {
        serviceCollection.AddSingleton<IHostedService, TImplementation>();
        return serviceCollection;
    }
}
