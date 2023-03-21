using CreativeCoders.Core.Collections;
using CreativeCoders.Daemon;
using CreativeCoders.Simba.Server.Core.SubModules;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core;

[UsedImplicitly]
public class SimbaServer : IDaemonService
{
    private readonly ISubModule[] _subModules;
    
    private readonly MqttServer _mqttServer;
    
    private readonly ILogger<SimbaServer> _logger;

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
