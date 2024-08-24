using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public class Poser2DTagsLibraryTests
{
	[Fact]
	public void ShouldCreateTag()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		dataSet.Tags.Should().Contain(tag);
	}

	[Fact]
	public void ShouldCreateMultipleTags()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var tag3 = dataSet.Tags.CreateTag("3");
		dataSet.Tags.Should().Contain([tag1, tag2, tag3]);
	}

	[Fact]
	public void ShouldNotCreateTagWithOccupiedName()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		Assert.ThrowsAny<Exception>(() => dataSet.Tags.CreateTag("1"));
		dataSet.Tags.Should().Contain(tag1);
		dataSet.Tags.Should().HaveCount(1);
	}

	[Fact]
	public void ShouldDeleteTag()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		dataSet.Tags.DeleteTag(tag);
		dataSet.Tags.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeleteTagWithItems()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(0, 0, 1, 1), [], []);
		Assert.ThrowsAny<Exception>(() => dataSet.Tags.DeleteTag(tag));
		dataSet.Tags.Should().Contain(tag);
	}
}