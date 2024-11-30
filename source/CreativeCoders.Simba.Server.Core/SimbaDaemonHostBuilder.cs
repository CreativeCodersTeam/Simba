using CreativeCoders.Core.IO;
using CreativeCoders.Daemon;
using CreativeCoders.Simba.Server.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CreativeCoders.Simba.Server.Core;

public static class SimbaDaemonHostBuilder
{
    private const string ConfigFileName = "/etc/simba.conf";

    public static IDaemonHostBuilder CreateSimbaDaemonHostBuilder(string[] args)
    {
        return DaemonHostBuilder
            .CreateBuilder<SimbaServer>()
            .WithArgs(args)
            .ConfigureServices(ConfigureServices)
            .ConfigureHostBuilder(x =>
                {
                    x.UseSerilog((context, conf) => conf.WriteTo.Console());

                    if (FileSys.File.Exists(ConfigFileName))
                    {
                        x.ConfigureAppConfiguration((_, configBuilder) =>
                            configBuilder.AddJsonFile("/etc/simba.conf"));
                    }
                }
            );
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSimbaServer();
        
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        
        services.Configure<ServerOptions>(config.GetSection("simba"));
    }
}
