using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
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

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed, ReplicationSession session)
	{
		var typedPackedTag = (PackablePoserTag)packed;
		var tag = (PoserTag)base.ReplicateTag(library, packed, session);
		foreach (var packedKeyPointTag in typedPackedTag.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPoint(typedPackedTag.Name);
			keyPointTag.Color = packedKeyPointTag.Color;
			session.Tags.Add((library.DataSet, packedKeyPointTag.Id), keyPointTag);
		}
		return tag;
	}
	
	protected sealed override PoserWeights ReplicateWeights(WeightsLibrary library, PackableWeights weights, ReplicationSession session)
	{
		var typedLibrary = (WeightsLibrary<TTag, TKeyPointTag>)library;
		var typedWeights = (PackablePoserWeights)weights;
		var tags = GetTags(library.DataSet, typedWeights, session);
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}

	private static IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> GetTags(DataSet dataSet, PackablePoserWeights weights, ReplicationSession session)
	{
		foreach (var (tagId, keyPointTagIds) in weights.Tags)
		{
			var tag = (TTag)session.Tags[(dataSet, tagId)];
			var keyPointTags = GetKeyPointTags(keyPointTagIds, dataSet, session);
			yield return (tag, keyPointTags);
		}
	}

	private static IEnumerable<TKeyPointTag> GetKeyPointTags(IEnumerable<byte> keyPointTagIds, DataSet dataSet, ReplicationSession session)
	{
		return keyPointTagIds.Select(keyPointTagId => (TKeyPointTag)session.Tags[(dataSet, keyPointTagId)]);
	}
}