using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests;

internal static class Utilities
{
	public static Image CreateImage()
	{
		ImageSet set = new();
		return set.CreateImage();
	}

	public static TagsLibrary<Tag> CreateTagsLibrary()
	{
		return new ClassifierDataSet().TagsLibrary;
	}

	public static Weights CreateWeights(params IEnumerable<Tag> tags)
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