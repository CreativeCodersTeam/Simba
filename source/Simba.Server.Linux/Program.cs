
using Microsoft.Extensions.Hosting;
using Simba.Server.Core;

await SimbaDaemonHostBuilder.CreateSimbaDaemonHostBuilder(args)
    .ConfigureHostBuilder(x => x.UseSystemd())
    .Build()
    .RunAsync()
    .ConfigureAwait(false);
