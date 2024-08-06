using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableDetectorAsset))]
[MemoryPackUnion(1, typeof(SerializablePoser2DAsset))]
[MemoryPackUnion(2, typeof(SerializableClassifierAsset))]
internal abstract partial class SerializableAsset
{
	public Id ScreenshotId { get; }
	public AssetUsage Usage { get; }

	protected SerializableAsset(Id screenshotId, AssetUsage usage)
	{
		ScreenshotId = screenshotId;
		Usage = usage;
	}
}