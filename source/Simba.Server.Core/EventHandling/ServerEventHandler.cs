namespace Simba.Server.Core.EventHandling;

public class ServerEventHandler<TArg>
{
    private readonly Func<ServerEventArg<TArg>, Task> _onExecuteAsync;

    public ServerEventHandler(Func<ServerEventArg<TArg>, Task> onExecuteAsync)
    {
        _onExecuteAsync = onExecuteAsync;
    }
    
    public Task ExecuteAsync(ServerEventArg<TArg> serverEventArg)
    {
        return _onExecuteAsync(serverEventArg);
    }

    public bool AlwaysExecute { get; set; }
}
