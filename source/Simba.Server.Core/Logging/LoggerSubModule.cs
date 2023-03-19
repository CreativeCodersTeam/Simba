using Microsoft.Extensions.Logging;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core.Logging;

public class LoggerSubModule : ISubModule
{
    private readonly ILogger<LoggerSubModule> _logger;
    
    public LoggerSubModule(ILogger<LoggerSubModule> logger, MqttServer mqttServer)
    {
        _logger = logger;
        
        mqttServer.ClientConnectedAsync += args => Log("Client connected: {@args}", args);
        
        mqttServer.ClientDisconnectedAsync += args => Log("Client disconnected: {@args}", args);

        mqttServer.ValidatingConnectionAsync +=
            args => Log("Client validation: ClientID='{ClientId}', Endpoint='{Endpoint}', UserName='{UserName}'",
                args.ClientId, args.Endpoint, args.UserName);
    }
    
    private Task Log(string logMessage, params object[] args)
    {
#pragma warning disable CA2254
        _logger.LogInformation(logMessage, args);
#pragma warning restore CA2254
        
        return Task.CompletedTask;
    }

    public string Name => nameof(LoggerSubModule);
}
