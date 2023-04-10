using System.Text;
using CreativeCoders.Simba.Server.Core.SubModules;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core.Logging;

public class LoggerSubModule : ISubModule
{
    private readonly ILogger<LoggerSubModule> _logger;
    
    public LoggerSubModule(ILogger<LoggerSubModule> logger, MqttServer mqttServer)
    {
        _logger = logger;
        
        mqttServer.ClientConnectedAsync += args => Log("Client connected: {@Args}", args);
        
        mqttServer.ClientDisconnectedAsync += args => Log("Client disconnected: {@Args}", args);

        mqttServer.ValidatingConnectionAsync +=
            args => Log("Client validation: ClientID='{ClientId}', Endpoint='{Endpoint}', UserName='{UserName}'",
                args.ClientId, args.Endpoint, args.UserName);

        mqttServer.ClientSubscribedTopicAsync += args => Log("Topic subscribed: {@Args}", args);
        
        mqttServer.ClientUnsubscribedTopicAsync += args => Log("Topic unsubscribed: {@Args}", args);

        mqttServer.InterceptingPublishAsync += LogPublish;
    }

    private Task LogPublish(InterceptingPublishEventArgs args)
    {
        const string text = "Publish: {Topic} -> {Payload}";

        var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

        return Log(text, args.ApplicationMessage.Topic, payload);
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
