using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Classifier;

[MemoryPackable]
internal sealed partial class ClassifierAsset : Asset
{
	public Id TagId { get; set; }

	public ClassifierAsset(Id screenshotId, AssetUsage usage, Id tagId) : base(screenshotId, usage)
	{
		TagId = tagId;
	}
}