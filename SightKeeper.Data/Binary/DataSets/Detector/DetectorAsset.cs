using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
internal partial class DetectorAsset : Asset
{
	public ImmutableArray<DetectorItem> Items { get; }

	public DetectorAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<DetectorItem> items) : base(screenshotId, usage)
	{
		Items = items;
	}
}