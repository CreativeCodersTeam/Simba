using Microsoft.Extensions.Logging;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core.Logging;

public class LoggerSubModule : ISubModule
{
    private readonly ILogger<LoggerSubModule> _logger;

    public LoggerSubModule(ILogger<LoggerSubModule> logger)
    {
        _logger = logger;
    }
    
    public void Init(ServerController serverController)
    {
        serverController.Server.ClientConnectedAsync += args => Log("Client connected: {@args}", args);;

        serverController.Server.ClientDisconnectedAsync += args => Log("Client disconnected: {@args}", args);
    }

    private Task Log(string logMessage, params object[] args)
    {
#pragma warning disable CA2254
        _logger.LogInformation(logMessage, args);
#pragma warning restore CA2254
        
        return Task.CompletedTask;
    }
}
