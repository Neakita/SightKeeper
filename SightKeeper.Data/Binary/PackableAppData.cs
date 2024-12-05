using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets;

namespace SightKeeper.Data.Binary;

[MemoryPackable]
internal sealed partial class PackableAppData
{
	public ImmutableArray<PackableDataSet> DataSets { get; }
	public ApplicationSettings ApplicationSettings { get; }

	public PackableAppData(
		ImmutableArray<PackableDataSet> dataSets,
		ApplicationSettings applicationSettings)
	{
		DataSets = dataSets;
		ApplicationSettings = applicationSettings;
	}
}