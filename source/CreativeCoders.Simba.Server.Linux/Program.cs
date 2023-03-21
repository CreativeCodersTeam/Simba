using CreativeCoders.Daemon.Linux;
using CreativeCoders.Simba.Server.Core;

await SimbaDaemonHostBuilder.CreateSimbaDaemonHostBuilder(args)
    .WithDefinitionFile("daemon.json")
    .UseSystemd()
    .Build()
    .RunAsync()
    .ConfigureAwait(false);
