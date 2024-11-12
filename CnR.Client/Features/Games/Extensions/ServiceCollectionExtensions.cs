using CnR.Client.Features.Games;
using CnR.Client.Features.Games.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IGame, Game>();
        return serviceCollection;
    }
}
