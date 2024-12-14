using MemoryPack;
using SightKeeper.Data.Binary.Model;
using SightKeeper.Data.Binary.Model.DataSets;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public required IReadOnlyCollection<PackableScreenshotsLibrary> ScreenshotsLibraries { get; init; }
	public required IReadOnlyCollection<PackableDataSet> DataSets { get; init; }
	public required ApplicationSettings ApplicationSettings { get; init; }
}