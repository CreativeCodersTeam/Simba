using CreativeCoders.Simba.Server.Core.AccessControl;
using CreativeCoders.Simba.Server.Core.Logging;
using CreativeCoders.Simba.Server.Core.Retaining;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Simba.Server.Core.SubModules;

public static class SubModulesServiceCollectionExtensions
{
    public static void AddSubModules(this IServiceCollection services)
    {
        services.AddSingleton<ISubModule, LoggerSubModule>();
        
        services.AddSingleton<ISubModule, AccessControlSubModule>();

        services.AddSingleton<ISubModule, RetainSubModule>();
        
        services.AddSingleton<IAuthenticator, NoAuthAuthenticator>();
    }
}
