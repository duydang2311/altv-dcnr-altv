using CnR.Server.Features.Accounts.Abstractions;
using CnR.Server.Features.Accounts.Events;
using CnR.Server.Features.Accounts.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScript<SignInScript>()
            .AddSingleton<IAccountLoggedInEvent, AccountLoggedInEvent>();
        return serviceCollection;
    }
}
