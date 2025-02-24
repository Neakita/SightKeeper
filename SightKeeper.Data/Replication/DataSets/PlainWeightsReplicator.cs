using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Replication.DataSets;

internal static class PlainWeightsReplicator
{
	public static void ReplicateWeights(PlainWeightsLibrary weightsLibrary, TagsLibrary<Tag> tagsLibrary, IReadOnlyCollection<PackablePlainWeights> packableWeights)
	{
		foreach (var weights in packableWeights)
		{
			var composition = CompositionReplicator.ReplicateComposition(weights.Composition);
			var tags = ReplicateTags(tagsLibrary, weights.TagsIndexes);
			weightsLibrary.CreateWeights(weights.CreationTimestamp, weights.ModelSize, weights.Metrics, weights.Resolution, composition, tags);
		}
	}

	private static IEnumerable<Tag> ReplicateTags(TagsLibrary<Tag> tagsLibrary, IReadOnlyCollection<byte> tagsIndexes)
	{
		return tagsIndexes.Select(index => tagsLibrary.Tags[index]);
	}
}