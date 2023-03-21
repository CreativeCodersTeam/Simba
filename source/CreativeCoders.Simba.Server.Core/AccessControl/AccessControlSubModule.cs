using CreativeCoders.Simba.Server.Core.SubModules;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace CreativeCoders.Simba.Server.Core.AccessControl;

public class AccessControlSubModule : ISubModule
{
    private readonly IAuthenticator _authenticator;

    public AccessControlSubModule(IAuthenticator authenticator, MqttServer mqttServer)
    {
        _authenticator = authenticator;
        
        mqttServer.ValidatingConnectionAsync += ServerOnValidatingConnectionAsync;
    }
    
    private async Task ServerOnValidatingConnectionAsync(ValidatingConnectionEventArgs arg)
    {
        var authResponse = await _authenticator
            .AuthenticateAsync(
                new AuthRequest
                {
                    UserName = arg.UserName,
                    Password = arg.Password
                })
            .ConfigureAwait(false);

        if (!authResponse.IsAllowed)
        {
            arg.ReasonCode = MqttConnectReasonCode.NotAuthorized;
        }
    }

    public string Name => nameof(AccessControlSubModule);
}
