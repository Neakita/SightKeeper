using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model;
using SightKeeper.Data.Binary.Model.DataSets;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public ApplicationSettings ApplicationSettings { get; }
	public ImmutableArray<PackableGame> Games { get; }
	public ImmutableArray<PackableDataSet> DataSets { get; }

	public PackableAppData(
		ApplicationSettings applicationSettings,
		ImmutableArray<PackableGame> games,
		ImmutableArray<PackableDataSet> dataSets)
	{
		ApplicationSettings = applicationSettings;
		Games = games;
		DataSets = dataSets;
	}
}