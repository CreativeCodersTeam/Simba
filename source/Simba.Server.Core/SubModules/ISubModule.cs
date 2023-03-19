using MQTTnet.Server;

namespace Simba.Server.Core.SubModules;

public interface ISubModule
{
    void Init(ServerController serverController);
}
