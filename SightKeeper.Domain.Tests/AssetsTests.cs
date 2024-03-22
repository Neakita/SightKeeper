using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Tests;

public sealed class AssetsTests
{
    [Fact]
    public void ShouldDeleteItems()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = dataSet.Assets.MakeAsset(screenshot);
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var item = asset.CreateItem(itemClass, new Bounding());
        asset.Items.Should().Contain(item);
        asset.DeleteItem(item);
        asset.Items.Should().NotContain(item);
    }

    [Fact]
    public void ShouldNotDeleteItemWhichIsNotInAsset()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot1 = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var screenshot2 =  screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset1 = dataSet.Assets.MakeAsset(screenshot1);
        var asset2 = dataSet.Assets.MakeAsset(screenshot2);
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var item = asset1.CreateItem(itemClass, new Bounding());
        Assert.ThrowsAny<Exception>(() => asset2.DeleteItem(item));
    }
}