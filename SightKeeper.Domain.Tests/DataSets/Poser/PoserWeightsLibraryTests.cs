using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Tests.DataSets.Poser;

public sealed class PoserWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimplePoserWeightsDataAccess weightsDataAccess = new();
		var tagsBuilder = ImmutableDictionary.CreateBuilder<PoserTag, ImmutableHashSet<KeyPointTag>>();
		tagsBuilder.Add(tag, ImmutableHashSet<KeyPointTag>.Empty);
		var weights = weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), tagsBuilder.ToImmutable());
		dataSet.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		PoserDataSet dataSet = new("", 320);
		SimplePoserWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>>.Empty));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithWrongAssociatedKeyPointTags()
	{
		PoserDataSet dataSet = new("", 320);
		var tag1 = dataSet.Tags.CreateTag("1");
		tag1.CreateKeyPoint("1.1");
		var tag2 = dataSet.Tags.CreateTag("2");
		var keyPoint2 = tag2.CreateKeyPoint("2.1");
		SimplePoserWeightsDataAccess weightsDataAccess = new();
		var tagsBuilder = ImmutableDictionary.CreateBuilder<PoserTag, ImmutableHashSet<KeyPointTag>>();
		tagsBuilder.Add(tag1, ImmutableHashSet.Create(keyPoint2));
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), tagsBuilder.ToImmutable()));
		dataSet.Weights.Should().BeEmpty();
	}
}