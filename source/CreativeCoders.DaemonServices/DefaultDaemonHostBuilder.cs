using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CreativeCoders.DaemonServices;

public class DefaultDaemonHostBuilder<TDaemonService> : IDaemonHostBuilder
    where TDaemonService : class, IDaemonService
{
    private string[]? _args;

    private readonly List<Action<IServiceCollection>> _configureServicesActions;

    private readonly List<Action<IHostBuilder>> _configureHostBuilderActions;
    
    private Type? _installerType;

    public DefaultDaemonHostBuilder()
    {
        _configureServicesActions = new List<Action<IServiceCollection>>();
        _configureHostBuilderActions = new List<Action<IHostBuilder>>();
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

    public IDaemonHostBuilder ConfigureHostBuilder(Action<IHostBuilder> configureHostBuilder)
    {
        _configureHostBuilderActions.Add(configureHostBuilder);

        return this;
    }

    public IDaemonHostBuilder WithInstaller<TInstaller>() where TInstaller : class, IDaemonInstaller
    {
        _installerType = typeof(TInstaller);

        return this;
    }

    public IDaemonHost Build()
    {
        var builder = _args != null
            ? Host.CreateDefaultBuilder(_args)
            : Host.CreateDefaultBuilder();
        
        _configureHostBuilderActions
            .ForEach(configureHostBuilder => configureHostBuilder(builder));

        builder.ConfigureServices((_, services) =>
        {
            services.AddHostedService<DaemonWorker>();
            
            services.TryAddSingleton<IDaemonService, TDaemonService>();
            
            _configureServicesActions
                .ForEach(configureServices => configureServices(services));
        });

        return new DaemonHost(builder.Build(), _args ?? Array.Empty<string>(), _installerType);
    }
}
