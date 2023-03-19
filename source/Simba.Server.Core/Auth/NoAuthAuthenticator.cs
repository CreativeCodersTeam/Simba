namespace Simba.Server.Core.Auth;

public class NoAuthAuthenticator : IAuthenticator
{
    public Task<AuthResponse> AuthenticateAsync(AuthRequest authRequest)
    {
        return Task.FromResult(new AuthResponse { IsAllowed = true });
    }
}
