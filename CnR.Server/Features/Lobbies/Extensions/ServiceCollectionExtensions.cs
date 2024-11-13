using CnR.Server.Features.Lobbies;
using CnR.Server.Features.Lobbies.Abstractions;
using CnR.Server.Features.Lobbies.Events;
using CnR.Server.Features.Lobbies.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddLobbyFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<ILobbyFactory, LobbyFactory>()
            .AddSingleton<ILobbyCreatedEvent, LobbyCreatedEvent>()
            .AddTransient<ILobbyPlayerAddedEvent, LobbyPlayerAddedEvent>()
            .AddTransient<ILobbyPlayerRemovedEvent, LobbyPlayerRemovedEvent>()
            .AddScript<PopulateLobbiesScript>();
        return serviceCollection;
    }
}
