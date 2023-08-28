using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training;

public sealed class CLITrainerArguments
{
    public static readonly Dictionary<ModelSize, string> ModelSizes = new()
    {
        { ModelSize.Nano, "yolov8n" },
        { ModelSize.Small, "yolov8s" },
        { ModelSize.Medium, "yolov8m" },
        { ModelSize.Large, "yolov8l" },
        { ModelSize.XLarge, "yolov8x" }
    };
    
    public string Data { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public uint Epochs { get; set; } = 100;
    public bool Resume { get; set; }
    public bool AMP { get; set; } = true;

    public CLITrainerArguments(string data, string model, uint epochs, bool resume = false, bool amp = true)
    {
        Data = data;
        Model = model;
        Epochs = epochs;
        Resume = resume;
        AMP = amp;
    }

    public CLITrainerArguments(string data, ModelSize modelSize, uint epochs, bool resume = false, bool amp = true)
    {
        Data = data;
        Model = ModelSizes[modelSize];
        Epochs = epochs;
        Resume = resume;
        AMP = amp;
    }

    public override string ToString() => string.Join(' ', GetParameters());

    private IEnumerable<string> GetParameters()
    {
        Guard.IsNotEmpty(Data);
        Guard.IsNotEmpty(Model);
        yield return "yolo detect train";
        yield return $"data={Data}";
        yield return $"model={Model}";
        yield return $"epochs={Epochs}";
        if (Resume)
            yield return "resume=true";
        if (!AMP)
            yield return "amp=false";
    }
}