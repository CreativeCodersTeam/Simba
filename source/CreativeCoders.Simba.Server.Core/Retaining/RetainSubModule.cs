using CreativeCoders.Simba.Server.Core.SubModules;
using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core.Retaining;

public class RetainSubModule : ISubModule
{
    public RetainSubModule(MqttServer mqttServer)
    {
        mqttServer.RetainedMessageChangedAsync += ServerOnRetainedMessageChangedAsync;
        
        mqttServer.RetainedMessagesClearedAsync += ServerOnRetainedMessagesClearedAsync;
        
        mqttServer.LoadingRetainedMessageAsync += ServerOnLoadingRetainedMessageAsync;
    }

    private async Task ServerOnLoadingRetainedMessageAsync(LoadingRetainedMessagesEventArgs arg)
    {
        //throw new NotImplementedException();
    }

    private async Task ServerOnRetainedMessagesClearedAsync(EventArgs arg)
    {
        //throw new NotImplementedException();
    }

    private async Task ServerOnRetainedMessageChangedAsync(RetainedMessageChangedEventArgs arg)
    {
        //throw new NotImplementedException();
    }

    public string Name => nameof(RetainSubModule);
}
