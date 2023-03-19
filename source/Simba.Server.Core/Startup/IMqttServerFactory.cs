using MQTTnet.Server;

namespace Simba.Server.Core.Startup;

public interface IMqttServerFactory
{
    MqttServer CreateServer();
}