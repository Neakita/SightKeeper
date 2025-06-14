using MemoryPack;
using SightKeeper.Data.Model.DataSets;
using PackableImageSet = SightKeeper.Data.Model.Images.PackableImageSet;

namespace SightKeeper.Data;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public required IReadOnlyCollection<PackableImageSet> ImageSets { get; init; }
	public required IReadOnlyCollection<PackableDataSet> DataSets { get; init; }
}