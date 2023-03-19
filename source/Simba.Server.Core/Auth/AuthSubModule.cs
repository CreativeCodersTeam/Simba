using MQTTnet.Protocol;
using MQTTnet.Server;
using Simba.Server.Core.SubModules;

namespace Simba.Server.Core.Auth;

public class AuthSubModule : ISubModule
{
    private readonly IAuthenticator _authenticator;

    public AuthSubModule(IAuthenticator authenticator, MqttServer mqttServer)
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

    public string Name => nameof(AuthSubModule);
}
