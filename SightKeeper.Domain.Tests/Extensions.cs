using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests;

internal static class Extensions
{
	public static Image CreateImage(this ImageSet set)
	{
		return set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}

	public static Weights CreateWeights(this WeightsLibrary library, params IEnumerable<Tag> tags)
	{
		return library.CreateWeights(Model.UltralyticsYoloV11, DateTimeOffset.UtcNow, ModelSize.Nano,
			new WeightsMetrics(), new Vector2<ushort>(320, 320), null, tags);
	}
}