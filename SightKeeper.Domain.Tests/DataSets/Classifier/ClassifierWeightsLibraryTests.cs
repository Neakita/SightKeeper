﻿using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		SimpleWeightsDataAccess weightsDataAccess = new();
		var weights = weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag1, tag2]);
		dataSet.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		ClassifierDataSet dataSet = new();
		SimpleWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), []));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithOneTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.Tags.CreateTag("");
		SimpleWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag]));
		dataSet.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.Tags.CreateTag("1");
		var tag2 = dataSet.Tags.CreateTag("2");
		SimpleWeightsDataAccess weightsDataAccess = new();
		Assert.ThrowsAny<Exception>(() => weightsDataAccess.CreateWeights(dataSet.Weights, [], ModelSize.Nano, new WeightsMetrics(), [tag1, tag1, tag2]));
		dataSet.Weights.Should().BeEmpty();
	}
}