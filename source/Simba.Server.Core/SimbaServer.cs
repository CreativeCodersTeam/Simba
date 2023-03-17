using CreativeCoders.Core.Collections;
using CreativeCoders.Daemon;
using JetBrains.Annotations;
using MQTTnet;
using MQTTnet.Server;
using Simba.Server.Core.Logging;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core;

[UsedImplicitly]
public class SimbaServer : IDaemonService
{
    private readonly IEnumerable<ISubModule> _subModules;
    
    private MqttServer? _mqttServer;

    public SimbaServer(IEnumerable<ISubModule> subModules)
    {
        _subModules = subModules;
    }
    
    private async Task<MqttServer> StartServerAsync()
    {
        var mqttFactory = new MqttFactory();

        // Due to security reasons the "default" endpoint (which is unencrypted) is not enabled by default!
        var mqttServerOptions = mqttFactory
            .CreateServerOptionsBuilder()
            .WithDefaultEndpoint()
            .Build();
        
        var server = mqttFactory.CreateMqttServer(mqttServerOptions);
        
        _subModules.ForEach(subModule => subModule.Init(server));

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
