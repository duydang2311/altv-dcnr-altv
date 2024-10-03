using CnR.Server.Characters;
using CnR.Server.Players.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlayerFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICharacterFactory, CharacterFactory>();
        return serviceCollection;
    }
}
