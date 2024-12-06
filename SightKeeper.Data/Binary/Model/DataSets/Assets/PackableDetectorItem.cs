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
	public byte TagIndex { get; }
	public Bounding Bounding { get; }

	public PackableDetectorItem(byte tagIndex, Bounding bounding)
	{
		TagIndex = tagIndex;
		Bounding = bounding;
	}
}