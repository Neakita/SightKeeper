using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="ClassifierAsset"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierAsset : PackableAsset
{
	public byte TagId { get; }

	public PackableClassifierAsset(AssetUsage usage, uint screenshotId, byte tagId) : base(usage, screenshotId)
	{
		TagId = tagId;
	}
}