using FluentAssertions;
using SightKeeper.Domain.Model.Exceptions;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Model.Tests.DetectorDataSet;

public sealed class AssetsTests
{
    [Fact]
    public void ShouldDeleteItems()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshot);
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var item = asset.CreateItem(itemClass, new Bounding());
        asset.Items.Should().Contain(item);
        asset.DeleteItem(item);
        asset.Items.Should().NotContain(item);
    }

    [Fact]
    public void ShouldNotDeleteItemWhichIsNotInAsset()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot1 = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        var screenshot2 =  dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        var asset1 = dataSet.MakeAsset(screenshot1);
        var asset2 = dataSet.MakeAsset(screenshot2);
        var itemClass = dataSet.CreateItemClass("Item class", 0);
        var item = asset1.CreateItem(itemClass, new Bounding());
        Assert.Throws<DetectorItemException>(() => asset2.DeleteItem(item));
    }
}