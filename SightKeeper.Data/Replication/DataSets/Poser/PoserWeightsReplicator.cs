using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Replication.DataSets.Poser;

internal static class PoserWeightsReplicator
{
	public static void ReplicateWeights(PoserWeightsLibrary weightsLibrary, TagsLibrary<PoserTag> tagsLibrary, IEnumerable<PackablePoserWeights> packableWeights)
	{
		foreach (var weights in packableWeights)
		{
			var composition = CompositionReplicator.ReplicateComposition(weights.Composition);
			var tags = ReplicateTags(tagsLibrary, weights.TagsIndexes);
			weightsLibrary.CreateWeights(weights.CreationTimestamp, weights.ModelSize, weights.Metrics, weights.Resolution, composition, tags);
		}
	}

	private static Dictionary<PoserTag, IReadOnlyCollection<Tag>> ReplicateTags(TagsLibrary<PoserTag> tagsLibrary, IReadOnlyDictionary<byte, IReadOnlyCollection<byte>> tagsIndexes)
	{
		Dictionary<PoserTag, IReadOnlyCollection<Tag>> tags = new();
		foreach (var (poserTagIndex, keyPointTagsIndexes) in tagsIndexes)
		{
			var poserTag = tagsLibrary.Tags[poserTagIndex];
			List<Tag> keyPointTags = new(keyPointTagsIndexes.Count);
			foreach (var keyPointIndex in keyPointTagsIndexes)
			{
				var keyPointTag = poserTag.KeyPointTags[keyPointIndex];
				keyPointTags.Add(keyPointTag);
			}
			tags.Add(poserTag, keyPointTags);
		}
		return tags;
	}
}