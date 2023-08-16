using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class WeightsTests
{
    [Fact]
    public void ShouldAddWeights()
    {
        DetectorDataSet dataSet = new("Model");
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), DateTime.Now,
            new ModelConfig(string.Empty, string.Empty, ModelType.Detector), DateTime.Now);
        dataSet.WeightsLibrary.Weights.Should().Contain(weights);
    }

    [Fact]
    public void ShouldNotCreateWithAssetFromDifferentModel()
    {
        DetectorDataSet model1 = new("Model 1");
        DetectorDataSet model2 = new("Model 2");
        var screenshot1 = model1.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var screenshot2 = model2.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset1 = model1.MakeAsset(screenshot1);
        var asset2 = model2.MakeAsset(screenshot2);
        Assert.Throws<ArgumentException>(() => model1.WeightsLibrary.CreateWeights(Array.Empty<byte>(), DateTime.Now,
            new ModelConfig(string.Empty, string.Empty, ModelType.Detector), 0, 0, null, new[] { asset1, asset2 }));
    }
}