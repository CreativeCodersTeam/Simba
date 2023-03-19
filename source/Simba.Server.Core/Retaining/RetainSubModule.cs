using MQTTnet.Server;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core.Retaining;

public class RetainSubModule : ISubModule
{
    public void Init(ServerController serverController)
    {
        serverController.Server.RetainedMessageChangedAsync += ServerOnRetainedMessageChangedAsync;
        
        serverController.Server.RetainedMessagesClearedAsync += ServerOnRetainedMessagesClearedAsync;
        
        serverController.Server.LoadingRetainedMessageAsync += ServerOnLoadingRetainedMessageAsync;
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
