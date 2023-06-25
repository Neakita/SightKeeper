using System.Reactive;
using Serilog;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Images;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Tests;

public class DarknetTrainerTests
{
    [Fact]
    public void ShouldRunTrainer()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Seq("http://localhost:5341").CreateLogger();
        DetectorModel model = new("Test model");
        ItemClass itemClass = new("TestItemClass");
        model.ItemClasses.Add(itemClass);
        var image = File.ReadAllBytes("Samples/320screenshot.png");
        var assets = Enumerable.Repeat(Unit.Default, 200).Select(_ => new DetectorAsset(new Screenshot(new Image(image)))
        {
            Items = new List<DetectorItem>
            {
                new(itemClass, new BoundingBox(0, 0, 1, 1))
            }
        }).ToList();
        model.Assets = assets;
        ModelConfig config = new("Yolo V3", File.ReadAllText("Samples/YoloV3.config"), ModelType.Detector);
        model.Config = config;
        DetectorTrainer trainer = new(new DarknetDetectorAdapter(new DetectorImagesExporter(),
            new DarknetProcessImplementation(), new DarknetDetectorOutputParser()));
        trainer.Model = model;
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
        trainer.TrainAsync(cancellationTokenSource.Token).GetAwaiter().GetResult();
    }
}