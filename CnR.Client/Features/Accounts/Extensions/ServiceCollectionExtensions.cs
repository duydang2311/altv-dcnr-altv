using CnR.Client.Features.Accounts.Scripts;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountFeatures(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScript<SignInScript>();
        return serviceCollection;
    }
}
