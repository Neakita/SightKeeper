using FluentAssertions;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests.DataSets.Weights;

public sealed class WeightsLibraryTests
{
	[Fact]
	public void ShouldCreateWeights()
	{
		DataSet dataSet = new ClassifierDataSet();
		var tag1 = dataSet.TagsLibrary.CreateTag("1");
		var tag2 = dataSet.TagsLibrary.CreateTag("2");
		var weights = dataSet.WeightsLibrary.CreateWeights(Model.UltralyticsYoloV11, DateTime.UtcNow, ModelSize.Nano, new WeightsMetrics(), new Vector2<ushort>(320, 320), null, [tag1, tag2]);
		dataSet.WeightsLibrary.Weights.Should().Contain(weights);
	}
}