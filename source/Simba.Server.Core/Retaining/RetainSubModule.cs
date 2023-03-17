using MQTTnet.Server;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core.Retaining;

public class RetainSubModule : ISubModule
{
    public void Init(MqttServer server)
    {
        server.RetainedMessageChangedAsync += ServerOnRetainedMessageChangedAsync;
        
        server.RetainedMessagesClearedAsync += ServerOnRetainedMessagesClearedAsync;
        
        server.LoadingRetainedMessageAsync += ServerOnLoadingRetainedMessageAsync;
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
}
