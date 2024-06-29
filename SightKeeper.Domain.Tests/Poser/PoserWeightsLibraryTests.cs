using FluentAssertions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Tests.Poser;

public sealed class PoserWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimplePoserWeightsDataAccess weightsDataAccess = new();
		var weights = weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), [tag]);
		dataSet.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		PoserDataSet dataSet = new("", 320);
		SimplePoserWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), []));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		PoserDataSet dataSet = new("", 320);
		var tag = dataSet.Tags.CreateTag("");
		SimplePoserWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet, [], ModelSize.Nano, new WeightsMetrics(), [tag, tag]));
		dataSet.Weights.Should().BeEmpty();
	}
}