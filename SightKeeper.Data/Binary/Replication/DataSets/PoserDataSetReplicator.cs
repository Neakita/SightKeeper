using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal abstract class PoserDataSetReplicator<TTag, TKeyPointTag, TDataSet> : DataSetReplicator<TDataSet>
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
	where TDataSet : DataSet, new()
{
	protected PoserDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess, ReplicationSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected override PoserTag ReplicateTag(TagsLibrary library, PackableTag packed)
	{
		var typedPackedTag = (PackablePoserTag)packed;
		var tag = (PoserTag)base.ReplicateTag(library, packed);
		foreach (var packedKeyPointTag in typedPackedTag.KeyPointTags)
		{
			var keyPointTag = tag.CreateKeyPoint(packedKeyPointTag.Name);
			keyPointTag.Color = packedKeyPointTag.Color;
			Session.Tags.Add((library.DataSet, packedKeyPointTag.Id), keyPointTag);
		}
		return tag;
	}
	
	protected sealed override PoserWeights ReplicateWeights(WeightsLibrary library, PackableWeights weights)
	{
		var typedLibrary = (WeightsLibrary<TTag, TKeyPointTag>)library;
		var typedWeights = (PackablePoserWeights)weights;
		var tags = GetTags(library.DataSet, typedWeights);
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}

	private IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> GetTags(DataSet dataSet, PackablePoserWeights weights)
	{
		foreach (var (tagId, keyPointTagIds) in weights.Tags)
		{
			var tag = (TTag)Session.Tags[(dataSet, tagId)];
			var keyPointTags = GetKeyPointTags(keyPointTagIds, dataSet, Session);
			yield return (tag, keyPointTags);
		}
	}

	private static IEnumerable<TKeyPointTag> GetKeyPointTags(IEnumerable<byte> keyPointTagIds, DataSet dataSet, ReplicationSession session)
	{
		return keyPointTagIds.Select(keyPointTagId => (TKeyPointTag)session.Tags[(dataSet, keyPointTagId)]);
	}
}