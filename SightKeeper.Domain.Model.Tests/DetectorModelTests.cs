using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests;

public class DetectorModelTests
{
    [Fact]
    public void ShouldBeAbleChangeResolutionAndMessageIsNull()
    {
        DetectorModel model = new("Test model");
        model.GetCanChangeResolution(out var message).Should().BeTrue();
        message.Should().BeNull();
    }
    
    [Fact]
    public void ShouldChangeResolutionWhenNoScreenshotsAndAssets()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        model.Resolution.Should().Be(firstResolution);
        model.Resolution = secondResolution;
        model.Resolution.Should().Be(secondResolution);
    }
    
    [Fact]
    public void ShouldCannotChangeResolutionAndMessageIsNotNullWhenHaveScreenshots()
    {
        DetectorModel model = new("Test model");
        model.Screenshots.Add(new Screenshot(new Image(Array.Empty<byte>())));
        model.GetCanChangeResolution(out var message).Should().BeFalse();
        message.Should().NotBeNull();
    }

    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveScreenshots()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        model.Screenshots.Add(new Screenshot(new Image(Array.Empty<byte>())));
        model.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            model.Resolution = secondResolution;
        });
        model.Resolution.Should().Be(firstResolution);
    }
    
    [Fact]
    public void ShouldCannotChangeResolutionAndMessageIsNotNullWhenHaveAssets()
    {
        DetectorModel model = new("Test model");
        model.Assets.Add(new DetectorAsset(model, new Screenshot(new Image(Array.Empty<byte>()))));
        model.GetCanChangeResolution(out var message).Should().BeFalse();
        message.Should().NotBeNull();
    }

    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveAssets()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        model.Assets.Add(new DetectorAsset(model, new Screenshot(new Image(Array.Empty<byte>()))));
        model.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            model.Resolution = secondResolution;
        });
        model.Resolution.Should().Be(firstResolution);
    }
}