using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Replication.DataSets;

internal static class WeightsReplicator
{
	public static void ReplicateWeights(WeightsLibrary weightsLibrary, TagsContainer<Tag> tagsContainer,
		IReadOnlyCollection<PackableWeights> packableWeights)
	{
		foreach (var weights in packableWeights)
		{
			var composition = CompositionReplicator.ReplicateComposition(weights.Composition);
			var tags = ReplicateTags(tagsContainer, weights.TagsIndexes);
			if (tagsContainer is TagsContainer<PoserTag> poserTagsContainer)
			{
				var keyPointTags = ReplicateKeyPointTags(poserTagsContainer, weights.KeyPointTagsLocations);
				tags = tags.Concat(keyPointTags);
			}
			weightsLibrary.CreateWeights(weights.Model, weights.CreationTimestamp, weights.ModelSize, weights.Metrics, weights.Resolution, composition, tags);
		}
	}

	private static IEnumerable<Tag> ReplicateTags(TagsContainer<Tag> tagsContainer, IEnumerable<byte> tagsIndexes)
	{
		return tagsIndexes.Select(index => tagsContainer.Tags[index]);
	}

	private static IEnumerable<Tag> ReplicateKeyPointTags(TagsContainer<PoserTag> tagsContainer, IEnumerable<KeyPointTagLocation> locations)
	{
		foreach (var location in locations)
		{
			var poserTag = tagsContainer.Tags[location.PoserTagIndex];
			yield return poserTag.KeyPointTags[location.KeyPointTagIndex];
		}
	}
}