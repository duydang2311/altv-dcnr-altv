using CnR.Server.Features.Uis;
using CnR.Server.Features.Uis.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddUiFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUi, Ui>();
        return serviceCollection;
    }
}
