using SightKeeper.Data.Model.DataSets.Compositions;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Data.Replication.DataSets;

internal static class CompositionReplicator
{
	public static ImageComposition? ReplicateComposition(PackableComposition? composition) => composition switch
	{
		null => null,
		PackableFixedTransparentComposition fixedTransparent => new FixedTransparentImageComposition(
			fixedTransparent.MaximumScreenshotsDelay, fixedTransparent.Opacities),
		PackableFloatingTransparentComposition floatingTransparent => new FloatingTransparentImageComposition(
			floatingTransparent.MaximumScreenshotsDelay, floatingTransparent.SeriesDuration,
			floatingTransparent.PrimaryOpacity, floatingTransparent.MinimumOpacity),
		_ => throw new ArgumentOutOfRangeException(nameof(composition))
	};
}