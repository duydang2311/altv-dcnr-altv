using CnR.Server.Features.Characters;
using CnR.Server.Features.Characters.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlayerFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICharacterFactory, CharacterFactory>();
        return serviceCollection;
    }
}
