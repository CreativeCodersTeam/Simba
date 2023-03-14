using Microsoft.Extensions.Hosting;

namespace CreativeCoders.DaemonServices;

public class DaemonHost : IDaemonHost
{
    private readonly IHost _host;
    
    private readonly string[] _args;
    
    private readonly Type? _installerType;

    private string _installArg = "--install";

    private string _uninstallArg = "--uninstall";

    public DaemonHost(IHost host, string[] args, Type? installerType)
    {
        _host = host;
        _args = args;
        _installerType = installerType;
    }

    public async Task RunAsync()
    {
        if (_args.Contains(_installArg))
        {
            CreateInstaller()?.Install();
        }

        if (_args.Contains(_uninstallArg))
        {
            CreateInstaller()?.Uninstall();
        }
        
        await _host.RunAsync().ConfigureAwait(false);
    }

    private IDaemonInstaller? CreateInstaller()
    {
        if (_installerType == null)
        {
            return null;
        }

        return Activator.CreateInstance(_installerType) as IDaemonInstaller;
    }
}
