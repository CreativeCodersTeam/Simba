namespace CreativeCoders.Simba.Server.Core.AccessControl;

public interface IAuthenticator
{
    Task<AuthResponse> AuthenticateAsync(AuthRequest authRequest);
}
