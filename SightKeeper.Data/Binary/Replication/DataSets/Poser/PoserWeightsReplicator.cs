using SightKeeper.Data.Binary.Conversion.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets.Poser;

internal static class PoserWeightsReplicator
{
	public static void ReplicateWeights(PoserWeightsLibrary weightsLibrary, TagsLibrary<PoserTag> tagsLibrary, IEnumerable<PackablePoserWeights> packableWeights)
	{
		foreach (var weights in packableWeights)
		{
			var composition = CompositionReplicator.ReplicateComposition(weights.Composition);
			var tags = ReplicateTags(tagsLibrary, weights.TagsIndexes);
			weightsLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, composition, tags);
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