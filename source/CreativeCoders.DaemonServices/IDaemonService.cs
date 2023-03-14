namespace CreativeCoders.DaemonServices;

public interface IDaemonService
{
    Task StartAsync();

    Task StopAsync();
}
