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
        var screenshot1 = model1.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var screenshot2 = model2.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset1 = model1.MakeAssetFromScreenshot(screenshot1);
        var asset2 = model2.MakeAssetFromScreenshot(screenshot2);
        Assert.Throws<ArgumentException>(() => new ModelWeights(model1, 0, Array.Empty<byte>(), new List<Asset> { asset1, asset2 }));
    }
}