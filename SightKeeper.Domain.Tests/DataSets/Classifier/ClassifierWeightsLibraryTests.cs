using FluentAssertions;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Classifier;

public sealed class ClassifierWeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, [tag1, tag2]);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		ClassifierDataSet dataSet = new();
		Assert.ThrowsAny<Exception>(() => dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, []));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithOneTag()
	{
		ClassifierDataSet dataSet = new();
		var tag = dataSet.TagsLibrary.CreateTag("");
		Assert.ThrowsAny<Exception>(() => dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, [tag]));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		ClassifierDataSet dataSet = new();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		Assert.ThrowsAny<Exception>(() => dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, [tag1, tag1, tag2]));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}
}