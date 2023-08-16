using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class CooperationTests
{
    [Fact]
    public void ShouldMakeAssetAndKeepItInScreenshots()
    {
        DetectorModel model = new("Test model");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = model.MakeAsset(screenshot);
        model.ScreenshotsLibrary.Screenshots.Should().Contain(screenshot);
        model.Assets.Should().Contain(asset);
    }
    
    [Fact]
    public void ShouldNotDeleteItemClassWithAssetItems()
    {
        DetectorModel model = new("Model");
        var itemClass = model.CreateItemClass("Item class");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = model.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        Assert.Throws<InvalidOperationException>(() => model.DeleteItemClass(itemClass));
    }
}