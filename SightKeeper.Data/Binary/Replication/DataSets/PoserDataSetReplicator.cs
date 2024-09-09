using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal abstract class PoserDataSetReplicator<TTag, TKeyPointTag> : DataSetReplicator
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	protected PoserDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed, ImmutableDictionary<(byte, byte?), Tag>.Builder lookupBuilder)
	{
		var typedPackedTag = (PackablePoserTag)packed;
		var tag = (PoserTag)base.ReplicateTag(library, packed, lookupBuilder);
		foreach (var packedKeyPointTag in typedPackedTag.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPoint(typedPackedTag.Name);
			keyPointTag.Color = packedKeyPointTag.Color;
			lookupBuilder.Add((typedPackedTag.Id, packedKeyPointTag.Id), keyPointTag);
		}
		return tag;
	}
	
	protected sealed override PoserWeights ReplicateWeights(WeightsLibrary library, PackableWeights weights, TagGetter getTag)
	{
		var typedLibrary = (WeightsLibrary<TTag, TKeyPointTag>)library;
		var typedWeights = (PackablePoserWeights)weights;
		var tags = GetTags(typedWeights, getTag);
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}

	private static IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> GetTags(PackablePoserWeights weights, TagGetter getTag)
	{
		foreach (var (tagId, keyPointTagIds) in weights.Tags)
		{
			var tag = (TTag)getTag(tagId);
			var keyPointTags = GetKeyPointTags(tagId, keyPointTagIds, getTag);
			yield return (tag, keyPointTags);
		}
	}

	private static IEnumerable<TKeyPointTag> GetKeyPointTags(byte tagId, IEnumerable<byte> keyPointTagIds, TagGetter getTag)
	{
		foreach (var keyPointTagId in keyPointTagIds)
			yield return (TKeyPointTag)getTag(tagId, keyPointTagId);
	}
}