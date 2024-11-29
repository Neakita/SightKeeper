using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public class Poser2DTagTests
{
	[Fact]
	public void ShouldNotChangeTagNameToOccupied()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		Assert.ThrowsAny<Exception>(() => tag2.Name = "1");
		tag1.Name.Should().Be("1");
		tag2.Name.Should().Be("2");
	}

	[Fact]
	public void ShouldSetTagNameToDeletedTagName()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		dataSet.TagsLibrary.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}

	[Fact]
	public void ShouldAddNewKeyPointTagToPoserTagWithAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(), []);
		var keyPointTag = tag.CreateKeyPoint("");
		tag.KeyPoints.Should().Contain(keyPointTag);
	}

	[Fact]
	public void ShouldDeleteKeyPointTagWithAssociatedKeyPoints()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag1 = tag.CreateKeyPoint("1");
		var keyPointTag2 = tag.CreateKeyPoint("2");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		var item = asset.CreateItem(tag, new Bounding(), []);
		var keyPoint1 = item.CreateKeyPoint(keyPointTag1, new Vector2<double>(0.1, 0.2));
		var keyPoint2 = item.CreateKeyPoint(keyPointTag2, new Vector2<double>(0.3, 0.4));
		tag.DeleteKeyPoint(keyPointTag1);
		item.KeyPoints.Should().Contain(keyPoint2).And.NotContain(keyPoint1);
	}

	[Fact]
	public void ShouldAddNewPointToTagWithoutAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(), []);
		var keyPoint = tag2.CreateKeyPoint("");
		tag2.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldDeletePointOfTagWithoutAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPoint("");
		var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(DateTime.Now, new Vector2<ushort>(320, 320), out _);
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(), []);
		tag2.DeleteKeyPoint(keyPoint2);
		tag2.KeyPoints.Should().BeEmpty();
	}
}