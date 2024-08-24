﻿using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Detector;

public sealed class DetectorWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		var weights = dataSet.Weights.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), [tag]);
		dataSet.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		DetectorDataSet dataSet = new();
		Assert.ThrowsAny<Exception>(() => dataSet.Weights.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), []));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		DetectorDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		Assert.ThrowsAny<Exception>(() => dataSet.Weights.CreateWeights(DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), [tag, tag]));
		dataSet.Weights.Should().BeEmpty();
	}
}