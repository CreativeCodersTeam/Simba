using CreativeCoders.Simba.Server.Core.Startup;
using CreativeCoders.Simba.Server.Core.SubModules;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Simba.Server.Core;

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
