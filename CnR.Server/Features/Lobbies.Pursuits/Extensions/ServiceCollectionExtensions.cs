using CnR.Server.Features.Lobbies.Pursuits.Abstractions;
using CnR.Server.Features.Lobbies.Pursuits.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPursuitLobbyFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<PursuitLobbyStatusChangedEvent>()
            .AddScript<SetupPursuitLobbyScript>();
        return serviceCollection;
    }
}
