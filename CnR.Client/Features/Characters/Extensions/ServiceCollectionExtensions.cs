using CnR.Client.Features.Characters.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCharacterFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScript<SelectGamemodeScript>();
        return serviceCollection;
    }
}
