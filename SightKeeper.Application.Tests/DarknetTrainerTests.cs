using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData;
using Serilog;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
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
        var screenshots = Enumerable.Repeat(Unit.Default, 64).Select(_ => new DetectorScreenshot(new Image(image))
        {
            IsAsset = true,
            Items = new ObservableCollection<DetectorItem>
            {
                new(itemClass, new BoundingBox(0, 0, 1, 1))
            }
        }).ToList();
        model.DetectorScreenshots.AddRange(screenshots);
        ModelConfig config = new("Yolo V3", File.ReadAllText("Samples/YoloV3.config"), ModelType.Detector);
        model.Config = config;
        DetectorTrainer trainer = new(new DarknetHelper(new DetectorImagesExporter()), new DarknetDetectorOutputParser());
        trainer.Model = model;
        trainer.TrainAsync().GetAwaiter().GetResult();
    }
}