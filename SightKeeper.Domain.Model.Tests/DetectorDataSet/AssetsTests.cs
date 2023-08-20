using FluentAssertions;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class AssetsTests
{
    [Fact]
    public void ShouldDeleteItems()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        var itemClass = dataSet.CreateItemClass("Item class");
        var item = asset.CreateItem(itemClass, new Bounding());
        asset.Items.Should().Contain(item);
        asset.DeleteItem(item);
        asset.Items.Should().NotContain(item);
    }

    [Fact]
    public void ShouldNotDeleteItemWhichIsNotInAsset()
    {
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot1 = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var screenshot2 =  dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset1 = dataSet.MakeAsset(screenshot1);
        var asset2 = dataSet.MakeAsset(screenshot2);
        var itemClass = dataSet.CreateItemClass("Item class");
        var item = asset1.CreateItem(itemClass, new Bounding());
        Assert.Throws<ArgumentException>(() => asset2.DeleteItem(item));
    }
}