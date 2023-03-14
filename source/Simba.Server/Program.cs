using CreativeCoders.DaemonServices;
using Microsoft.Extensions.Hosting;
using Simba.Server.Core;

var host = DaemonHostBuilder
    .CreateBuilder<SimbaServer>()
    .WithArgs(args)
    .Build();

await host.RunAsync().ConfigureAwait(false);
