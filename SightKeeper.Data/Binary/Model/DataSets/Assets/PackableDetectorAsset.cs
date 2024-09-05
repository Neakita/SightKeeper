using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="DetectorAsset"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableDetectorAsset : PackableItemsAsset<PackableDetectorItem>
{
	public PackableDetectorAsset(
		AssetUsage usage,
		uint screenshotId,
		ImmutableArray<PackableDetectorItem> items)
		: base(usage, screenshotId, items)
	{
	}
}