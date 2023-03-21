using CreativeCoders.Simba.Server.Core.EventHandling;
using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core;

public class ServerController
{
    public ServerController(MqttServer server)
    {
        Server = server;

        ClientConnectedEvent = new EventController<ClientConnectedEventArgs>(
            server,
            (x, handler) =>
                x.ClientConnectedAsync += handler);
    }

    public MqttServer Server { get; }
    
    public EventController<ClientConnectedEventArgs> ClientConnectedEvent { get; set; }
}
