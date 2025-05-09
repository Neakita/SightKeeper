using FluentAssertions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets;

public sealed class WeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		var dataSet = CreateDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("");
		var weights = dataSet.WeightsLibrary.CreateWeights(tag1);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}

	[Fact]
	public void ShouldNotCreateWeightsWithNoTags()
	{
		var dataSet = CreateDataSet();
		Assert.Throws<ArgumentException>(() => dataSet.WeightsLibrary.CreateWeights());
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotCreateWeightsWithDuplicateTags()
	{
		var dataSet = CreateDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		Assert.Throws<DuplicateTagsException>(() => dataSet.WeightsLibrary.CreateWeights(tag1, tag1, tag2));
		dataSet.WeightsLibrary.Weights.Should().BeEmpty();
	}

	private static DataSet CreateDataSet()
	{
		return new DetectorDataSet();
	}
}