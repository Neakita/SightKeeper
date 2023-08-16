using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class ResolutionTests
{
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
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveScreenshots()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        model.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            model.Resolution = secondResolution;
        });
        model.Resolution.Should().Be(firstResolution);
    }
    
    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveAssets()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        model.MakeAsset(screenshot);
        model.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            model.Resolution = secondResolution;
        });
        model.Resolution.Should().Be(firstResolution);
    }
}