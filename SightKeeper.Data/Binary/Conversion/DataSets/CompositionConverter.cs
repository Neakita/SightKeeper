using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal static class CompositionConverter
{
	public static PackableComposition? ConvertComposition(ImageComposition? composition) => composition switch
	{
		null => null,
		FixedTransparentImageComposition fixedTransparent => new PackableFixedTransparentComposition
		{
			MaximumScreenshotsDelay = fixedTransparent.MaximumScreenshotsDelay,
			Opacities = fixedTransparent.Opacities
		},
		FloatingTransparentImageComposition floatingTransparent => new PackableFloatingTransparentComposition
		{
			MaximumScreenshotsDelay = floatingTransparent.MaximumScreenshotsDelay,
			SeriesDuration = floatingTransparent.SeriesDuration,
			PrimaryOpacity = floatingTransparent.PrimaryOpacity,
			MinimumOpacity = floatingTransparent.MinimumOpacity
		},
		_ => throw new ArgumentOutOfRangeException(nameof(composition), composition, null)
	};
}