using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DAssetTests
{
	[Fact]
	public void ShouldCreateItem()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(), [], []);
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameTag()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag, new Bounding(0, 0, 0.5, 0.5), [], []);
		var item2 = asset.CreateItem(tag, new Bounding(0, 0, 1, 1), [], []);
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateMultipleItemsWithSameBounding()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item1 = asset.CreateItem(tag1, new Bounding(), [], []);
		var item2 = asset.CreateItem(tag2, new Bounding(), [], []);
		asset.Items.Should().Contain([item1, item2]);
	}

	[Fact]
	public void ShouldCreateItemWithKeyPoints()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		tag.CreateKeyPoint("1");
		tag.CreateKeyPoint("2");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(), [new Vector2<double>(), new Vector2<double>()], []);
		asset.Items.Should().Contain(item);
	}

	[Fact]
	public void ShouldNotCreateItemWithNotEqualAmountOfKeyPoints()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		tag.CreateKeyPoint("1");
		tag.CreateKeyPoint("2");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		Assert.ThrowsAny<Exception>(() => asset.CreateItem(tag, new Bounding(), [new Vector2<double>()], []));
		asset.Items.Should().BeEmpty();
	}
}