namespace Simba.Server.Core.Options;

[Flags]
public enum ServerEndpoints
{
    Default,
    Tls,
    Websocket
}
