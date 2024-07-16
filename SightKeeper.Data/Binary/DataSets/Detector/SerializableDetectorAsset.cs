using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Detector;

[MemoryPackable]
public partial class SerializableDetectorAsset : SerializableAsset
{
	public IReadOnlyCollection<SerializableDetectorItem> Items { get; }

	public SerializableDetectorAsset(
		Id screenshotId,
		AssetUsage usage,
		IReadOnlyCollection<SerializableDetectorItem> items) : base(screenshotId, usage)
	{
		Items = items;
	}
}