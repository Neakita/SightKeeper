using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Services.Scoring.Export;

public sealed class TempExporter : IDisposable
{
    private const string TempDirectory = "Temp";
    
    public string Export(ModelConfig config, DetectorConfigParameters parameters) => Export(parameters.Deploy(config));
    public string Export(Weights weights) => Export(weights.Data);
    public string Export(IEnumerable<ItemClass> itemClasses)
    {
        var itemClassesNames = itemClasses.Select(itemClass => itemClass.Name);
        var content = string.Join('\n', itemClassesNames);
        return Export(content);
    }

    public void Dispose()
    {
        foreach (var file in _files)
            File.Delete(file);
    }

    private readonly List<string> _files = new();

    private string Export(string content)
    {
        var path = Path.Combine(TempDirectory, Guid.NewGuid().ToString());
        File.WriteAllText(path, content);
        _files.Add(path);
        return path;
    }

    private string Export(byte[] content)
    {
        var path = Path.Combine(TempDirectory, Guid.NewGuid().ToString());
        File.WriteAllBytes(path, content);
        _files.Add(path);
        return path;
    }
}