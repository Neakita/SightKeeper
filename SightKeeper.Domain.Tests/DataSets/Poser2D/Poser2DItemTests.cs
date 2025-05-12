using FluentAssertions;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DItemTests
{
	[Fact]
	public void ShouldClearKeyPointsWhenChangingTag()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var keyPointTag1 = tag1.CreateKeyPointTag("1.1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		tag2.CreateKeyPointTag("2.1");
		var image = Utilities.CreateImage();
		var asset = dataSet.AssetsLibrary.MakeAsset(image);
		var item = asset.MakeItem(tag1, new Bounding());
		item.CreateKeyPoint(keyPointTag1, new Vector2<double>());
		item.Tag = tag2;
		item.KeyPoints.Should().BeEmpty();
	}
}