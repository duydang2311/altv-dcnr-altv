using CnR.Server.Features.Characters;
using CnR.Server.Features.Characters.Abstractions;
using CnR.Server.Features.Characters.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCharacterFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICharacterFactory, CharacterFactory>().AddScript<SpawnCharacterScript>();
        return serviceCollection;
    }
}
