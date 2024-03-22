using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Domain.Tests;

public sealed class ItemClassesTests
{
    [Fact]
    public void ShouldNotBeAbleCreateTwoItemClassesWithTheSameName()
    {
        const string itemClassName = "Test item class";
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.CreateItemClass(itemClassName, 0);
        Assert.Throws<ArgumentException>(() => dataSet.CreateItemClass(itemClassName, 0));
    }

    [Fact]
    public void ShouldBelongToProperDataSet()
    {
	   var dataSet = DomainTestsHelper.NewDataSet;
	   var itemClass = dataSet.CreateItemClass("Test item class", 0);
	   itemClass.DataSet.Should().Be(dataSet);
	   dataSet.ItemClasses.Should().Contain(itemClass);
    }

    [Fact]
    public void ShouldContainRelatedItems()
    {
	    SimpleScreenshotsDataAccess screenshotsDataAccess = new();
	    var dataSet = DomainTestsHelper.NewDataSet;
	    var itemClass = dataSet.CreateItemClass("Test item class", 0);
	    var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
	    var asset = screenshot.MakeAsset();
	    var item = asset.CreateItem(itemClass, new Bounding());
	    itemClass.Items.Should().Contain(item);
    }

    [Fact]
    public void ShouldNotBeDeletedFromOtherDataSet()
    {
	    var dataSet1 = DomainTestsHelper.NewDataSet;
	    var dataSet2 = DomainTestsHelper.NewDataSet;
	    var itemClass = dataSet1.CreateItemClass("Test item class", 0);
	    Assert.Throws<ArgumentException>(() => dataSet2.DeleteItemClass(itemClass));
    }
}