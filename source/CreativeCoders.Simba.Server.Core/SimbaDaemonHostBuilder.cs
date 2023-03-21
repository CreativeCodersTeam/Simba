using CreativeCoders.Daemon;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CreativeCoders.Simba.Server.Core;

public static class SimbaDaemonHostBuilder
{
    public static IDaemonHostBuilder CreateSimbaDaemonHostBuilder(string[] args)
    {
        return DaemonHostBuilder
            .CreateBuilder<SimbaServer>()
            .WithArgs(args)
            .ConfigureServices(ConfigureServices)
            .ConfigureHostBuilder(x =>
                x.UseSerilog((context, conf) => conf.WriteTo.Console()));
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSimbaServer();
    }
}
