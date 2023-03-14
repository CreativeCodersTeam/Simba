namespace CreativeCoders.DaemonServices;

public static class DaemonHostBuilder
{
    public static IDaemonHostBuilder CreateBuilder<TDaemonService>()
        where TDaemonService : class, IDaemonService
    {
        return new DefaultDaemonHostBuilder<TDaemonService>();
    }
}
