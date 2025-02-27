using SightKeeper.Data.Model.DataSets.Compositions;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Data.Conversion.DataSets;

internal static class CompositionConverter
{
	public static PackableComposition? ConvertComposition(ImageComposition? composition) => composition switch
	{
		null => null,
		FixedTransparentImageComposition fixedTransparent => new PackableFixedTransparentComposition
		{
			MaximumDelay = fixedTransparent.MaximumDelay,
			Opacities = fixedTransparent.Opacities
		},
		FloatingTransparentImageComposition floatingTransparent => new PackableFloatingTransparentComposition
		{
			MaximumDelay = floatingTransparent.MaximumDelay,
			SeriesDuration = floatingTransparent.SeriesDuration,
			PrimaryOpacity = floatingTransparent.PrimaryOpacity,
			MinimumOpacity = floatingTransparent.MinimumOpacity
		},
		_ => throw new ArgumentOutOfRangeException(nameof(composition), composition, null)
	};
}