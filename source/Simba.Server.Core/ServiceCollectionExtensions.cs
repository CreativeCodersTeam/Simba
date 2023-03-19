using Microsoft.Extensions.DependencyInjection;
using Simba.Server.Core.Startup;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core;

public static class ServiceCollectionExtensions
{
    public static void AddSimbaServer(this IServiceCollection services)
    {
        services.AddSubModules();
        
        services.AddSingleton<IMqttServerFactory, MqttServerFactory>();

        services.AddSingleton(sp =>
        {
            var factory = sp.GetRequiredService<IMqttServerFactory>();

            return factory.CreateServer();
        });
    }
}
