using NSubstitute;
using Serilog;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
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
            var screenshot = model.ScreenshotsLibrary.CreateScreenshot(imageData, new Resolution());
            var asset = model.MakeAsset(screenshot);
            asset.CreateItem(itemClass, new BoundingBox(0, 0, 1, 1));
        }
        ModelConfig config = new("Yolo V3", await File.ReadAllTextAsync("Samples/YoloV3.config"), ModelType.Detector);
        var imageLoader = Substitute.For<ScreenshotImageLoader>();
        var assetsDataAccess = Substitute.For<DetectorAssetsDataAccess>();
        DetectorTrainer trainer = new(new DarknetDetectorAdapter(new DetectorImagesExporter(imageLoader, assetsDataAccess),
            new DarknetProcessImplementation(), new DarknetDetectorOutputParser()), new DbWeightsDataAccess(new AppDbContext()));
        trainer.Model = model;
        CancellationTokenSource cancellationTokenSource = new();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
        await trainer.TrainAsync(config, cancellationTokenSource.Token);
    }
}