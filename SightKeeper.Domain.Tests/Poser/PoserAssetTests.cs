using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Tests.Poser;

public sealed class PoserAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(), []);
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag, new Bounding(0, 0, 0.5, 0.5), []);
		var item2 = asset.CreateItem(tag, new Bounding(0, 0, 1, 1), []);
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameBounding()
	{
		PoserDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag1, new Bounding(), []);
		var item2 = asset.CreateItem(tag2, new Bounding(), []);
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateItemWithKeyPoints()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		tag.AddKeyPoint("1");
		tag.AddKeyPoint("2");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(), [new Vector2<double>(), new Vector2<double>()]);
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldNotCreateItemWithNotEqualAmountOfKeyPoints()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		tag.AddKeyPoint("1");
		tag.AddKeyPoint("2");
		SimplePoserScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => asset.CreateItem(tag, new Bounding(), [new Vector2<double>()]));
		asset.Items.Should().BeEmpty();
	}
}