using FluentAssertions;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests;

public sealed class DetectorModelTests
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
        model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
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
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
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
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        model.ScreenshotsLibrary.Screenshots.Should().OnlyContain(x => x == screenshot);
    }

    [Fact]
    public void ShouldNotAddDuplicateScreenshot()
    {
        DetectorModel model = new("Test model");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        Assert.Throws<ArgumentException>(() => model.ScreenshotsLibrary.AddScreenshot(screenshot));
    }

    [Fact]
    public void ShouldMakeAssetAndDeleteItFromScreenshots()
    {
        DetectorModel model = new("Test model");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset = model.MakeAssetFromScreenshot(screenshot);
        model.ScreenshotsLibrary.Screenshots.Should().BeEmpty();
        model.Assets.Should().Contain(asset);
    }

    [Fact]
    public void ShouldNotAddDuplicateItemClasses()
    {
        DetectorModel model = new("Model");
        ItemClass itemClass = new("Item class");
        model.AddItemClass(itemClass);
        Assert.Throws<InvalidOperationException>(() => model.AddItemClass(itemClass));
    }

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
    public void ShouldNotSetConfigForDifferentModelType()
    {
        DetectorModel model = new("Model");
        ModelConfig config = new("Config", string.Empty, ModelType.Classifier);
        Assert.Throws<ArgumentException>(() => model.Config = config);
    }

    [Fact]
    public void ShouldNotDeleteItemClassWithAssetItems()
    {
        DetectorModel model = new("Model");
        var itemClass = model.CreateItemClass("Item class");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset = model.MakeAssetFromScreenshot(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        Assert.Throws<InvalidOperationException>(() => model.DeleteItemClass(itemClass));
    }
}