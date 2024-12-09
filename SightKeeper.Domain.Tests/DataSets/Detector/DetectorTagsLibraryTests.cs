using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var tag3 = dataSet.TagsLibrary.CreateTag("3");
		dataSet.TagsLibrary.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.TagsLibrary.CreateTag("1"));
		dataSet.TagsLibrary.Tags.Should().Contain(tag1);
		dataSet.TagsLibrary.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		dataSet.TagsLibrary.DeleteTag(tag);
		dataSet.TagsLibrary.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteTagWithItems()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(0, 0, 1, 1));
		Assert.ThrowsAny<Exception>(() => dataSet.TagsLibrary.DeleteTag(tag));
		dataSet.TagsLibrary.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldDeleteTagWithoutItems()
	{
		ScreenshotsLibrary screenshotsLibrary = new();
		var screenshot = screenshotsLibrary.CreateScreenshot(DateTimeOffset.Now, new Vector2<ushort>(320, 320));
		DetectorDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(0, 0, 1, 1));
		dataSet.TagsLibrary.DeleteTag(tag2);
		dataSet.TagsLibrary.Tags.Should().Contain(tag1);
		dataSet.TagsLibrary.Tags.Should().NotContain(tag2);
	}
}