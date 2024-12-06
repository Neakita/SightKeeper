using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal static class PlainWeightsReplicator
{
	public static void ReplicateWeights(PlainWeightsLibrary weightsLibrary, TagsLibrary<Tag> tagsLibrary, ImmutableArray<PackablePlainWeights> packableWeights)
	{
		foreach (var weights in packableWeights)
		{
			var composition = CompositionReplicator.ReplicateComposition(weights.Composition);
			var tags = ReplicateTags(tagsLibrary, weights.TagsIndexes);
			weightsLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, composition, tags);
		}
	}

	private static IEnumerable<Tag> ReplicateTags(TagsLibrary<Tag> tagsLibrary, ImmutableArray<byte> tagsIndexes)
	{
		return tagsIndexes.Select(index => tagsLibrary.Tags[index]);
	}
}