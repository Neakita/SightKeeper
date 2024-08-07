using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		SimpleWeightsDataAccess weightsDataAccess = new();
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag]);
		dataSet.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		DetectorDataSet dataSet = new();
		SimpleWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), []));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		SimpleWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag, tag]));
		dataSet.Weights.Should().BeEmpty();
	}
}