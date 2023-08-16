using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class AssetsTests
{
    [Fact]
    public void ShouldDeleteItems()
    {
        DetectorModel model = new("Model");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = model.MakeAsset(screenshot);
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
        var screenshot1 = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var screenshot2 =  model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset1 = model.MakeAsset(screenshot1);
        var asset2 = model.MakeAsset(screenshot2);
        var itemClass = model.CreateItemClass("Item class");
        var item = asset1.CreateItem(itemClass, new BoundingBox());
        Assert.Throws<ArgumentException>(() => asset2.DeleteItem(item));
    }
}