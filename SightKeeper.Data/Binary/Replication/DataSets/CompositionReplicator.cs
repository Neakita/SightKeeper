using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal static class CompositionReplicator
{
	public static Composition? ReplicateComposition(PackableComposition? composition) => composition switch
	{
		null => null,
		PackableFixedTransparentComposition fixedTransparent => new FixedTransparentComposition(
			fixedTransparent.MaximumScreenshotsDelay, fixedTransparent.Opacities),
		PackableFloatingTransparentComposition floatingTransparent => new FloatingTransparentComposition(
			floatingTransparent.MaximumScreenshotsDelay, floatingTransparent.SeriesDuration,
			floatingTransparent.PrimaryOpacity, floatingTransparent.MinimumOpacity),
		_ => throw new ArgumentOutOfRangeException(nameof(composition))
	};
}