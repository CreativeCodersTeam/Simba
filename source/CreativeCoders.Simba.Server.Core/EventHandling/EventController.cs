using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core.EventHandling;

public class EventController<TArg>
{
    private readonly MqttServer _server;

    private List<ServerEventHandler<TArg>> _eventHandlers = new List<ServerEventHandler<TArg>>();

    public EventController(MqttServer server, Action<MqttServer, Func<TArg, Task>> addEvent)
    {
        _server = server;

        addEvent(server, OnEventAsync);
    }

    private async Task OnEventAsync(TArg arg)
    {
        var serverEventArg = new ServerEventArg<TArg>(arg);

        foreach (var eventHandler in _eventHandlers)
        {
            await eventHandler.ExecuteAsync(serverEventArg).ConfigureAwait(false);

            if (serverEventArg.IsHandled)
            {
                break;
            }
        }
    }

    // public ServerEventHandler<TArg> AddEventHandler(Func<ServerEventArg<TArg>, Task> onExecuteAsync)
    // {
    //     _eventHandlers.Add(new ServerEventHandler<TArg>(onExecuteAsync));
    // }
    //
    // public 
}
