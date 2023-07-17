namespace SightKeeper.Application.Config;

public interface ConfigEditor
{
    Task ApplyChanges(ConfigChange configChange, CancellationToken cancellationToken = default);
}