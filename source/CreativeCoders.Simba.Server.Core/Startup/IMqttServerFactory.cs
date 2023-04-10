using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core.Startup;

public interface IMqttServerFactory
{
    MqttServer CreateServer();
}