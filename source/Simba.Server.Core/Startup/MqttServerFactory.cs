using CreativeCoders.Core;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Server;
using Simba.Server.Core.Options;

namespace Simba.Server.Core.Startup;

public class MqttServerFactory : IMqttServerFactory
{
    private readonly ServerOptions _options;

    public MqttServerFactory(IOptions<ServerOptions> options)
    {
        _options = Ensure.NotNull(options, nameof(options)).Value;
    }
    
    public MqttServer CreateServer()
    {
        var mqttFactory = new MqttFactory();

        // Due to security reasons the "default" endpoint (which is unencrypted) is not enabled by default!
        var mqttServerOptionsBuilder = mqttFactory
            .CreateServerOptionsBuilder();

        if (_options.Endpoints.HasFlag(ServerEndpoints.Default))
        {
            mqttServerOptionsBuilder.WithDefaultEndpoint();
        }

        var mqttServerOptions = mqttServerOptionsBuilder.Build();
        
        return mqttFactory.CreateMqttServer(mqttServerOptions);
    }
}
