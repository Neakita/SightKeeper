using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.Training;

public sealed class CLITrainerArguments
{
    public static readonly Dictionary<Size, string> ModelSizes = new()
    {
        { Size.Nano, "yolov8n" },
        { Size.Small, "yolov8s" },
        { Size.Medium, "yolov8m" },
        { Size.Large, "yolov8l" },
        { Size.XLarge, "yolov8x" }
    };
    
    public string Data { get; set; }
    public string Model { get; set; }
    public uint Epochs { get; set; }
    public ushort Patience { get; set; }
    public ushort ImageSize { get; set; }
    public bool Resume { get; set; }
    public bool AMP { get; set; }

    public CLITrainerArguments(string data, string model, uint epochs, ushort patience, ushort imageSize, bool resume = false, bool amp = true)
    {
        Data = data;
        Model = model;
        Epochs = epochs;
        Patience = patience;
        ImageSize = imageSize;
        Resume = resume;
        AMP = amp;
    }

    public CLITrainerArguments(string data, Size size, uint epochs, ushort patience, ushort imageSize, bool resume = false, bool amp = true)
    {
        Data = data;
        Model = ModelSizes[size];
        ImageSize = imageSize;
        Epochs = epochs;
        Patience = patience;
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
        yield return $"imgsz={ImageSize}";
        yield return $"epochs={Epochs}";
        yield return $"patience={Patience}";
        if (Resume)
            yield return "resume=true";
        if (!AMP)
            yield return "amp=false";
    }
}