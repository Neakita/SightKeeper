using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Domain.Tests;

internal static class Utilities
{
	public static DomainImage CreateImage()
	{
		return new DomainImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320), new FakeImageDataLoader());
	}

	public static DomainTagsLibrary<DomainTag> CreateTagsLibrary()
	{
		return new DomainClassifierDataSet().TagsLibrary;
	}

	public static Weights CreateWeights(params IEnumerable<DomainTag> tags)
	{
		Weights weights = new(tags)
		{
			Model = Model.UltralyticsYoloV11,
			CreationTimestamp = DateTimeOffset.UtcNow,
			ModelSize = ModelSize.Nano,
			Metrics = new WeightsMetrics(),
			Resolution = new Vector2<ushort>(320, 320)
		};
		return weights;
	}
}