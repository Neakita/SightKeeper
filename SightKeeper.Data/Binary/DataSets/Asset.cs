using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.DataSets.Detector;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
[MemoryPackUnion(0, typeof(DetectorAsset))]
[MemoryPackUnion(1, typeof(Poser2DAsset))]
[MemoryPackUnion(2, typeof(ClassifierAsset))]
internal abstract partial class Asset
{
	public Id ScreenshotId { get; }
	public AssetUsage Usage { get; }

	protected Asset(Id screenshotId, AssetUsage usage)
	{
		ScreenshotId = screenshotId;
		Usage = usage;
	}
}