namespace Simba.Server.Core.EventHandling;

public class ServerEventArg<TArg>
{
    public ServerEventArg(TArg arg)
    {
        Arg = arg;
    }
    
    public TArg Arg { get; }

    public bool IsHandled { get; set; }
}
