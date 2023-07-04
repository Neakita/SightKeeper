using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests;

public sealed class DetectorAssetTests
{
    [Fact]
    public void ShouldDeleteItems()
    {
        DetectorModel model = new("Model");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset = model.MakeAssetFromScreenshot(screenshot);
        var itemClass = model.CreateItemClass("Item class");
        var item = asset.CreateItem(itemClass, new BoundingBox());
        asset.Items.Should().Contain(item);
        asset.DeleteItem(item);
        asset.Items.Should().NotContain(item);
    }

    [Fact]
    public void ShouldNotDeleteItemWhichIsNotInAsset()
    {
        DetectorModel model = new("Model");
        var screenshot1 = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var screenshot2 =  model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset1 = model.MakeAssetFromScreenshot(screenshot1);
        var asset2 = model.MakeAssetFromScreenshot(screenshot2);
        var itemClass = model.CreateItemClass("Item class");
        var item = asset1.CreateItem(itemClass, new BoundingBox());
        Assert.Throws<ArgumentException>(() => asset2.DeleteItem(item));
    }
}