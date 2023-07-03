using Serilog;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Services.Tests;

public sealed class DarknetTrainerTests
{
    [SkippableFact]
    public void ShouldRunTrainer()
    {
        Skip.If(true);
        Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Seq("http://localhost:5341").CreateLogger();
        DetectorModel model = new("Test model");
        var itemClass = model.CreateItemClass("Test item class");
        var imageData = File.ReadAllBytes("Samples/320screenshot.png");
        for (var i = 0; i < 200; i++)
        {
            Screenshot screenshot = new(new Image(imageData));
            model.AddScreenshot(screenshot);
            var asset = model.MakeAssetFromScreenshot(screenshot);
            asset.CreateItem(itemClass, new BoundingBox(0, 0, 1, 1));
        }
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