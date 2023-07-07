using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class CooperationTests
{
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