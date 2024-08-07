using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Domain.Tests.DataSets.Poser2D;

public sealed class Poser2DWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		Poser2DDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimpleWeightsDataAccess weightsDataAccess = new();
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [(tag, [])]);
		dataSet.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		Poser2DDataSet dataSet = new("", 320);
		SimpleWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), []));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithWrongAssociatedKeyPointTags()
	{
		Poser2DDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		tag1.CreateKeyPoint("1.1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPoint("2.1");
		SimpleWeightsDataAccess weightsDataAccess = new();
		var tagsBuilder = ImmutableDictionary.CreateBuilder<Poser2DTag, ImmutableHashSet<KeyPointTag2D>>();
		tagsBuilder.Add(tag1, ImmutableHashSet.Create(keyPoint2));
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [(tag1, [keyPoint2])]));
		dataSet.Weights.Should().BeEmpty();
	}
}