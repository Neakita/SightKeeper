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
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		Assert.ThrowsAny<Exception>(() => tag2.Name = "1");
		tag1.Name.Should().Be("1");
		tag2.Name.Should().Be("2");
	}

	[Fact]
	public void ShouldSetTagNameToDeletedTagName()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		dataSet.Tags.DeleteTag(tag1);
		tag2.Name = tag1.Name;
	}

	[Fact]
	public void ShouldNotAddNewPointToTagWithAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(), [], []);
		Assert.ThrowsAny<Exception>(() => tag.CreateKeyPoint(""));
		tag.KeyPoints.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotDeletePointOfTagWithAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var keyPoint = tag.CreateKeyPoint("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag, new Bounding(), [new Vector2<double>()], []);
		Assert.ThrowsAny<Exception>(() => tag.DeleteKeyPoint(keyPoint));
		tag.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldAddNewPointToTagWithoutAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(), [], []);
		var keyPoint = tag2.CreateKeyPoint("");
		tag2.KeyPoints.Should().Contain(keyPoint);
	}

	[Fact]
	public void ShouldDeletePointOfTagWithoutAssociatedItems()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPoint("");
		var screenshot = dataSet.Screenshots.CreateScreenshot(DateTime.Now, out _);
		var asset = dataSet.Assets.MakeAsset(screenshot);
		asset.CreateItem(tag1, new Bounding(), [], []);
		tag2.DeleteKeyPoint(keyPoint2);
		tag2.KeyPoints.Should().BeEmpty();
	}
}