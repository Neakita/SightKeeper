using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Config;

public interface ConfigEditor
{
    IObservable<ModelConfig> ConfigEdited { get; }

    Task ApplyChanges(ConfigChange configChange, CancellationToken cancellationToken = default);
}