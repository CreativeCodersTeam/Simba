using CreativeCoders.DaemonServices;
using MQTTnet;
using MQTTnet.Server;

namespace Simba.Server.Core;

public class SimbaServer : IDaemonService
{
    private MqttServer? _mqttServer;
    
    private async Task<MqttServer> StartServerAsync()
    {
        var mqttFactory = new MqttFactory();

        // Due to security reasons the "default" endpoint (which is unencrypted) is not enabled by default!
        var mqttServerOptions = mqttFactory
            .CreateServerOptionsBuilder()
            .WithDefaultEndpoint()
            .Build();
        
        var server = mqttFactory.CreateMqttServer(mqttServerOptions);
        
        await server.StartAsync();
        
        return server;
    }

    public async Task StartAsync()
    {
        _mqttServer = await StartServerAsync().ConfigureAwait(false);
    }

    public async Task StopAsync()
    {
        if (_mqttServer == null)
        {
            throw new InvalidOperationException("Server must be started");
        }
        
        await _mqttServer.StopAsync();
    }
}
