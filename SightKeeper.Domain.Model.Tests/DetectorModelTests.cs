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
        model.CanChangeResolution(out var message).Should().BeTrue();
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
        model.AddScreenshot(new Screenshot(new Image(Array.Empty<byte>())));
        model.CanChangeResolution(out var message).Should().BeFalse();
        message.Should().NotBeNull();
    }

    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveScreenshots()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        model.AddScreenshot(new Screenshot(new Image(Array.Empty<byte>())));
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
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        model.AddScreenshot(screenshot);
        model.MakeAssetFromScreenshot(screenshot);
        model.CanChangeResolution(out var message).Should().BeFalse();
        message.Should().NotBeNull();
    }

    [Fact]
    public void ShouldThrowExceptionOnResolutionChangeWhenHaveAssets()
    {
        Resolution firstResolution = new(64, 64);
        Resolution secondResolution = new(128, 128);
        DetectorModel model = new("Test model", firstResolution);
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        model.AddScreenshot(screenshot);
        model.MakeAssetFromScreenshot(screenshot);
        model.Resolution.Should().Be(firstResolution);
        Assert.Throws<InvalidOperationException>(() =>
        {
            model.Resolution = secondResolution;
        });
        model.Resolution.Should().Be(firstResolution);
    }

    [Fact]
    public void ShouldNotBeAbleCreateTwoItemClassesWithTheSameName()
    {
        const string itemClassName = "Test item class";
        DetectorModel model = new("Dummy model");
        model.CreateItemClass(itemClassName);
        Assert.Throws<InvalidOperationException>(() => model.CreateItemClass(itemClassName));
    }

    [Fact]
    public void ShouldAddScreenshot()
    {
        DetectorModel model = new("Test model");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        model.AddScreenshot(screenshot);
        model.Screenshots.Should().OnlyContain(x => x == screenshot);
    }

    [Fact]
    public void ShouldNotAddDuplicateScreenshot()
    {
        DetectorModel model = new("Test model");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        model.AddScreenshot(screenshot);
        Assert.Throws<InvalidOperationException>(() => model.AddScreenshot(screenshot));
    }

    [Fact]
    public void ShouldMakeAssetAndDeleteItFromScreenshots()
    {
        DetectorModel model = new("Test model");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        model.AddScreenshot(screenshot);
        var asset = model.MakeAssetFromScreenshot(screenshot);
        model.Screenshots.Should().BeEmpty();
        model.Assets.Should().Contain(asset);
    }

    [Fact]
    public void ShouldNotMakeAssetFromNotOwnedScreenshot()
    {
        DetectorModel model = new("Test model");
        Screenshot screenshot = new(new Image(Array.Empty<byte>()));
        Assert.Throws<InvalidOperationException>(() => model.MakeAssetFromScreenshot(screenshot));
    }
}