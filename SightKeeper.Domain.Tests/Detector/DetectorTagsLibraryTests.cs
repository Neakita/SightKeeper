using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.Detector;

public sealed class DetectorTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		DetectorDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		dataSet.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		DetectorDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var tag3 = dataSet.Tags.CreateTag("3");
		dataSet.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		DetectorDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.Tags.CreateTag("1"));
		dataSet.Tags.Should().Contain(tag1);
		dataSet.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		DetectorDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		dataSet.Tags.DeleteTag(tag);
		dataSet.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteTagWithItems()
	{
		DetectorDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(0, 0, 1, 1));
		Assert.ThrowsAny<Exception>(() => dataSet.Tags.DeleteTag(tag));
		dataSet.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldDeleteTagWithoutItems()
	{
		DetectorDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		SimpleScreenshotsDataAccess screenshotsDataAccess = new();
		var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, []);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(0, 0, 1, 1));
		dataSet.Tags.DeleteTag(tag2);
		dataSet.Tags.Should().Contain(tag1);
		dataSet.Tags.Should().NotContain(tag2);
	}
}