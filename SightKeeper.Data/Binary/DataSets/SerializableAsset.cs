using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(Detector.SerializableDetectorAsset))]
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