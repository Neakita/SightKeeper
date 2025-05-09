using System.Collections.ObjectModel;
using FluentAssertions;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, [tag]);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		Poser2DDataSet dataSet = new();
		Assert.ThrowsAny<Exception>(() => dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, ReadOnlyCollection<Tag>.Empty));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithKeyPointTagsWithoutItsOwner()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		tag1.CreateKeyPointTag("1.1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPointTag("2.1");
		var exception = Assert.Throws<KeyPointTagWithoutOwnerException>(() => dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, [tag1, keyPoint2]));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
		exception.ExpectedOwner.Should().Be(tag2);
		exception.KeyPointTag.Should().Be(keyPoint2);
	}

	[Fact]
	public void ShouldCreateWeightsWithKeyPointTag()
	{
		
	}
}