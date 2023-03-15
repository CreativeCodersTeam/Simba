using CreativeCoders.DaemonServices;
using Simba.Server.Core;

await DaemonHostBuilder
    .CreateBuilder<SimbaServer>()
    .WithArgs(args)
    .Build()
    .RunAsync()
    .ConfigureAwait(false);
