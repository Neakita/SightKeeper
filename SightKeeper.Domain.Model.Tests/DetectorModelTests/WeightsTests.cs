using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        DetectorModel model = new("Model");
        ModelWeights weights = new(model, 0, Array.Empty<byte>(), Enumerable.Empty<Asset>());
        model.WeightsLibrary.AddWeights(weights);
        model.WeightsLibrary.Weights.Should().Contain(weights);
    }
    
    [Fact]
    public void ShouldNotAddDuplicateWeights()
    {
        DetectorModel model = new("Model");
        ModelWeights weights = new(model, 0, Array.Empty<byte>(), Enumerable.Empty<Asset>());
        model.WeightsLibrary.AddWeights(weights);
        Assert.Throws<ArgumentException>(() => model.WeightsLibrary.AddWeights(weights));
    }
    
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