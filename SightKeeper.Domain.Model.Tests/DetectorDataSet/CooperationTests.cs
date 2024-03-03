using FluentAssertions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class CooperationTests
{
    [Fact]
    public void ShouldMakeAssetAndKeepItInScreenshots()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshot);
        dataSet.Screenshots.Screenshots.Should().Contain(screenshot);
        dataSet.Assets.Should().Contain(asset);
    }
    
    [Fact]
    public void ShouldNotDeleteItemClassWithAssetItems()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshot);
        asset.CreateItem(itemClass, new Bounding());
        Assert.Throws<InvalidOperationException>(() => dataSet.DeleteItemClass(itemClass));
    }
}