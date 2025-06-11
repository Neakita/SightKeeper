using FluentAssertions;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DItemTests
{
	[Fact]
	public void ShouldClearKeyPointsWhenChangingTag()
	{
		DomainPoser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var keyPointTag1 = tag1.CreateKeyPointTag("1.1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		tag2.CreateKeyPointTag("2.1");
		var image = Utilities.CreateImage();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag1);
		item.MakeKeyPoint(keyPointTag1);
		item.Tag = tag2;
		item.KeyPoints.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotSetKeyPointPositionToNotNormalized()
	{
		DomainPoser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var keyPointTag = tag.CreateKeyPointTag("");
		var image = Utilities.CreateImage();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag);
		var keyPoint = item.MakeKeyPoint(keyPointTag);
		Vector2<double> newPosition = new(1.1, 1.2);
		var exception = Assert.Throws<KeyPointPositionConstraintException>(() => keyPoint.Position = newPosition);
		exception.Value.Should().Be(newPosition);
		exception.KeyPoint.Should().Be(keyPoint);
	}
}