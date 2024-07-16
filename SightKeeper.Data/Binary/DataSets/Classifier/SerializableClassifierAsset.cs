using FlakeId;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Classifier;

internal sealed class SerializableClassifierAsset : SerializableAsset
{
	public Id TagId { get; set; }

	public SerializableClassifierAsset(Id screenshotId, AssetUsage usage, Id tagId) : base(screenshotId, usage)
	{
		TagId = tagId;
	}
}