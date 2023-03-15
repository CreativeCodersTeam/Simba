using CreativeCoders.DaemonServices.Definition;

namespace CreativeCoders.DaemonServices;

public interface IDaemonInstaller
{
    void Install(DaemonDefinition daemonDefinition);

    void Uninstall(DaemonDefinition daemonDefinition);
}
