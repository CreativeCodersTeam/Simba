using Microsoft.Extensions.DependencyInjection;
using Simba.Server.Core.Auth;
using Simba.Server.Core.Logging;
using Simba.Server.Core.Retaining;

namespace Simba.Server.Core.SubModules;

public static class SubModulesServiceCollectionExtensions
{
    public static void AddSubModules(this IServiceCollection services)
    {
        services.AddSingleton<ISubModule, LoggerSubModule>();

        services.AddSingleton<ISubModule, RetainSubModule>();

        services.AddSingleton<ISubModule, AuthSubModule>();

        services.AddSingleton<IAuthenticator, NoAuthAuthenticator>();
    }
}
