using System.Collections.Immutable;
using FluentAssertions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		Poser2DDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		var weights = dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag], [], null);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		Poser2DDataSet dataSet = new();
		Assert.ThrowsAny<Exception>(() => dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [], [], null));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithWrongAssociatedKeyPointTags()
	{
		Poser2DDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		tag1.CreateKeyPoint("1.1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPoint("2.1");
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Poser2DTag, ImmutableHashSet<KeyPointTag2D>>();
		tagsBuilder.Add(tag1, ImmutableHashSet.Create(keyPoint2));
		Assert.ThrowsAny<Exception>(() => dataSet.WeightsLibrary.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), [tag1], [keyPoint2], null));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}
}