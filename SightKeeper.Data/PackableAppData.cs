using MemoryPack;
using SightKeeper.Data.Model;
using SightKeeper.Data.Model.DataSets;

namespace SightKeeper.Data;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public required IReadOnlyCollection<PackableImageSet> ImageSets { get; init; }
	public required IReadOnlyCollection<PackableDataSet> DataSets { get; init; }
	public required ApplicationSettings ApplicationSettings { get; init; }
}