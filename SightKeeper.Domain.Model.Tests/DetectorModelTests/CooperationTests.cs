using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Tests.DetectorModelTests;

public sealed class CooperationTests
{
    [Fact]
    public void ShouldMakeAssetAndKeepItInScreenshots()
    {
        DetectorDataSet dataSet = new("Test model");
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        dataSet.ScreenshotsLibrary.Screenshots.Should().Contain(screenshot);
        dataSet.Assets.Should().Contain(asset);
    }
    
    [Fact]
    public void ShouldNotDeleteItemClassWithAssetItems()
    {
        DetectorDataSet dataSet = new("Model");
        var itemClass = dataSet.CreateItemClass("Item class");
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new BoundingBox());
        Assert.Throws<InvalidOperationException>(() => dataSet.DeleteItemClass(itemClass));
    }
}