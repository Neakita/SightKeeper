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
		foreach (var packable in packableWeights)
		{
			var composition = CompositionReplicator.ReplicateComposition(packable.Composition);
			var tags = ReplicateTags(tagsContainer, packable.TagsIndexes);
			if (tagsContainer is TagsContainer<PoserTag> poserTagsContainer)
			{
				var keyPointTags = ReplicateKeyPointTags(poserTagsContainer, packable.KeyPointTagsLocations);
				tags = tags.Concat(keyPointTags);
			}
			Weights weights = new(tags)
			{
				Model = packable.Model,
				CreationTimestamp = packable.CreationTimestamp,
				ModelSize = packable.ModelSize,
				Metrics = packable.Metrics,
				Resolution = packable.Resolution,
				Composition = composition
			};
			weightsLibrary.AddWeights(weights);
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