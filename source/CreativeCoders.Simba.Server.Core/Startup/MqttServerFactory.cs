using CreativeCoders.Core;
using CreativeCoders.Simba.Server.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core.Startup;

public class MqttServerFactory : IMqttServerFactory
{
    private readonly ILogger<MqttServerFactory> _logger;
    
    private readonly ServerOptions _options;

    public MqttServerFactory(IOptions<ServerOptions> options, ILogger<MqttServerFactory> logger)
    {
        _options = Ensure.NotNull(options, nameof(options)).Value;
        
        _logger = Ensure.NotNull(logger, nameof(logger));
    }
    
    public MqttServer CreateServer()
    {
        var mqttFactory = new MqttFactory();

        // Due to security reasons the "default" endpoint (which is unencrypted) is not enabled by default!
        var mqttServerOptionsBuilder = mqttFactory
            .CreateServerOptionsBuilder();

        if (_options.Endpoints.HasFlag(ServerEndpoints.Default))
        {
            _logger.LogInformation("Use default endpoint");
            mqttServerOptionsBuilder.WithDefaultEndpoint();
        }

        var mqttServerOptions = mqttServerOptionsBuilder.Build();
        
        return mqttFactory.CreateMqttServer(mqttServerOptions);
    }
}
