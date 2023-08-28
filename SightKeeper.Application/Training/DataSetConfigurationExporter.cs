using YamlDotNet.Serialization;

namespace SightKeeper.Application.Training;

public sealed class DataSetConfigurationExporter
{
    public Task Export(string path, DataSetConfigurationParameters parameters, CancellationToken cancellationToken = default)
    {
        var content = _serializer.Serialize(parameters);
        return File.WriteAllTextAsync(path, content, cancellationToken);
    }

    private readonly Serializer _serializer = new();
}