using CreativeCoders.Core.Collections;
using CreativeCoders.Daemon;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core;

[UsedImplicitly]
public class SimbaServer : IDaemonService
{
    private readonly ISubModule[] _subModules;
    
    private readonly MqttServer _mqttServer;
    
    private readonly ILogger<SimbaServer> _logger;

    private ServerController? _serverController;

    public SimbaServer(IEnumerable<ISubModule> subModules, MqttServer mqttServer,
        ILogger<SimbaServer> logger)
    {
        _subModules = subModules.ToArray();
        _mqttServer = mqttServer;
        _logger = logger;

        _subModules.ForEach(x => _logger.LogInformation("Sub module {SubModuleName} loaded", x.Name));
    }
    
    public async Task StartAsync()
    {
        _logger.LogInformation("Simba server starting...");
        
        _serverController = new ServerController(_mqttServer);
        
        await _mqttServer.StartAsync();
        
        _logger.LogInformation("Simba server started");
    }

    public async Task StopAsync()
    {
        if (!_mqttServer.IsStarted)
        {
            throw new InvalidOperationException("Server must be started");
        }
        
        await _mqttServer.StopAsync();
    }
}
