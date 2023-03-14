using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.DaemonServices;

public interface IDaemonHostBuilder
{
    IDaemonHostBuilder WithArgs(string[] args);

    IDaemonHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

    IHost Build();
}
