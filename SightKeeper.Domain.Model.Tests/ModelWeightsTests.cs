using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests;

public sealed class ModelWeightsTests
{
    [Fact]
    public void ShouldNotCreateWithAssetFromDifferentModel()
    {
        DetectorModel model1 = new("Model 1");
        DetectorModel model2 = new("Model 2");
        Screenshot screenshot1 = new(new Image(Array.Empty<byte>()));
        Screenshot screenshot2 = new(new Image(Array.Empty<byte>()));
        model1.ScreenshotsLibrary.AddScreenshot(screenshot1);
        model2.ScreenshotsLibrary.AddScreenshot(screenshot2);
        var asset1 = model1.MakeAssetFromScreenshot(screenshot1);
        var asset2 = model2.MakeAssetFromScreenshot(screenshot2);
        Assert.Throws<ArgumentException>(() => new ModelWeights(model1, 0, Array.Empty<byte>(), new List<Asset> { asset1, asset2 }));
    }
}