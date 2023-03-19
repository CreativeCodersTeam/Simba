namespace Simba.Server.Core.Auth;

public interface IAuthenticator
{
    Task<AuthResponse> AuthenticateAsync(AuthRequest authRequest);
}
