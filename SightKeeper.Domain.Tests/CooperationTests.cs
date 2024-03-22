using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Tests;

public sealed class CooperationTests
{
    [Fact]
    public void ShouldMakeAssetAndKeepItInScreenshots()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = screenshot.MakeAsset();
        dataSet.Screenshots.Should().Contain(screenshot);
        dataSet.Assets.Should().Contain(asset);
    }
    
    [Fact]
    public void ShouldNotDeleteItemClassWithAssetItems()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = screenshot.MakeAsset();
        asset.CreateItem(itemClass, new Bounding());
        Assert.Throws<InvalidOperationException>(() => dataSet.DeleteItemClass(itemClass));
    }
}