using Moq;
using Serilog;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services.Tests;

public sealed class DarknetTrainerTests
{
    [SkippableFact]
    public async Task ShouldRunTrainer()
    {
        Skip.If(true);
        Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Seq("http://localhost:5341").CreateLogger();
        DetectorModel model = new("Test model");
        var itemClass = model.CreateItemClass("Test item class");
        var imageData = await File.ReadAllBytesAsync("Samples/320screenshot.png");
        for (var i = 0; i < 200; i++)
        {
            var screenshot = model.ScreenshotsLibrary.CreateScreenshot(imageData);
            var asset = model.MakeAsset(screenshot);
            asset.CreateItem(itemClass, new BoundingBox(0, 0, 1, 1));
        }
        ModelConfig config = new("Yolo V3", await File.ReadAllTextAsync("Samples/YoloV3.config"), ModelType.Detector);
        model.Config = config;
        Mock<ScreenshotImageLoader> imageLoader = new();
        Mock<DetectorAssetsDataAccess> assetsDataAccess = new();
        DetectorTrainer trainer = new(new DarknetDetectorAdapter(new DetectorImagesExporter(imageLoader.Object, assetsDataAccess.Object),
            new DarknetProcessImplementation(), new DarknetDetectorOutputParser()));
        trainer.Model = model;
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
        await trainer.TrainAsync(cancellationTokenSource.Token);
    }
}