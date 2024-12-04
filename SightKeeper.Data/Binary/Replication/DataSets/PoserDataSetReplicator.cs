using System.Collections.Immutable;
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
			var keyPointTag = tag.CreateKeyPointTag(packedKeyPointTag.Name);
			keyPointTag.Color = packedKeyPointTag.Color;
			Session.Tags.Add((library.DataSet, packedKeyPointTag.Id), keyPointTag);
		}
		return tag;
	}
	
	protected sealed override PoserWeights ReplicateWeights(WeightsLibrary library, PackableWeights weights)
	{
		var typedLibrary = (WeightsLibrary<TTag, TKeyPointTag>)library;
		GetTags(library.DataSet, weights, out var tags, out var keyPointTags);
		var composition = ReplicateComposition(weights.Composition);
		return typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags, keyPointTags, composition);
	}

	private void GetTags(DataSet dataSet, PackableWeights weights, out ImmutableArray<TTag> tags, out ImmutableArray<TKeyPointTag> keyPointTags)
	{
		var tagsBuilder = ImmutableArray.CreateBuilder<TTag>();
		var keyPointTagsBuilder = ImmutableArray.CreateBuilder<TKeyPointTag>();
		foreach (var tagId in weights.TagIds)
		{
			var tag = Session.Tags[(dataSet, tagId)];
			switch (tag)
			{
				case TTag poserTag:
					tagsBuilder.Add(poserTag);
					break;
				case TKeyPointTag keyPointTag:
					keyPointTagsBuilder.Add(keyPointTag);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(tag), tag, null);
			}
		}
		tags = tagsBuilder.DrainToImmutable();
		keyPointTags = keyPointTagsBuilder.DrainToImmutable();
	}
}