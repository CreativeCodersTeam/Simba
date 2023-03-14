using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.DaemonServices;

public class DefaultDaemonHostBuilder<TDaemonService> : IDaemonHostBuilder
    where TDaemonService : class, IDaemonService
{
    private string[]? _args;

    private readonly List<Action<IServiceCollection>> _configureServicesActions;

    public DefaultDaemonHostBuilder()
    {
        _configureServicesActions = new List<Action<IServiceCollection>>();
    }
    
    public IDaemonHostBuilder WithArgs(string[] args)
    {
        _args = args;

        return this;
    }

    public IDaemonHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        _configureServicesActions.Add(configureServices);

        return this;
    }

    public IHost Build()
    {
        var builder = _args != null
            ? Host.CreateDefaultBuilder(_args)
            : Host.CreateDefaultBuilder();

        builder.ConfigureServices((_, services) =>
        {
            services.AddHostedService<DaemonWorker>();
            
            services.TryAddSingleton<IDaemonService, TDaemonService>();
            
            _configureServicesActions
                .ForEach(configureServices => configureServices(services));
        });

        return builder.Build();
    }
}
