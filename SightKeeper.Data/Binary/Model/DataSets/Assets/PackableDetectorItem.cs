using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="DetectorItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableDetectorItem
{
	public byte TagId { get; }
	public Bounding Bounding { get; }

	public PackableDetectorItem(byte tagId, Bounding bounding)
	{
		TagId = tagId;
		Bounding = bounding;
	}
}