using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class CooperationTests
{
    [Fact]
    public void ShouldMakeAssetAndKeepItInScreenshots()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        dataSet.ScreenshotsLibrary.Screenshots.Should().Contain(screenshot);
        dataSet.Assets.Should().Contain(asset);
    }
    
    [Fact]
    public void ShouldNotDeleteItemClassWithAssetItems()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var itemClass = dataSet.CreateItemClass("Item class");
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new Bounding());
        Assert.Throws<InvalidOperationException>(() => dataSet.DeleteItemClass(itemClass));
    }
}