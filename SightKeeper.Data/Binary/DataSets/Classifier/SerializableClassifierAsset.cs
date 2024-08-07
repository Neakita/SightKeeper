using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Classifier;

[MemoryPackable]
internal sealed partial class SerializableClassifierAsset : SerializableAsset
{
	public Id TagId { get; set; }

	public SerializableClassifierAsset(Id screenshotId, AssetUsage usage, Id tagId) : base(screenshotId, usage)
	{
		TagId = tagId;
	}
}