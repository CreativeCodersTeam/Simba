using CreativeCoders.Daemon;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Simba.Server.Core;
using Simba.Server.Core.SubModules;

await DaemonHostBuilder
    .CreateBuilder<SimbaServer>()
    .WithArgs(args)
    .ConfigureServices(ConfigureServices)
    .ConfigureHostBuilder(x => x.UseSerilog((context, conf) => conf.WriteTo.Console()))
    .Build()
    .RunAsync()
    .ConfigureAwait(false);

void ConfigureServices(IServiceCollection services)
{
    services.AddSubModules();
}
